using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Administration;
using LoftyRoomsModel.Ads;
using LoftyRoomsModel.Bookings;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.Partners;
using LoftyRoomsModel.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text;

namespace LoftyRoomsDAL.Customers
{
    public class CustomerDAL
    {
        ApplicationDBContext db;
        private readonly IConfiguration configuration;
        public CustomerDAL(ApplicationDBContext _db, IConfiguration _configuration)
        {
            configuration = _configuration;
            db = _db;
        }

        public CustomerListDto Login(string Email, string Password, string AndroidFcmToken, string IosFcmToken)
        {
            try
            {
                var customer = db.Customers.Where(x => (x.Email == Email || x.Mobile == Email) && x.Password == Password && x.IsDeleted == false).Select(g => new CustomerListDto
                {
                    CustomerId = g.CustomerId,
                    CustomerName = g.CustomerName,
                    Email = g.Email,
                    Mobile = g.Mobile,
                    ProfileImage = g.ProfileImage
                }).FirstOrDefault();
                if (AndroidFcmToken != "")
                {
                    var customerAndroid = db.Customers.Where(x => (x.Email == Email || x.Mobile == Email) && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
                    customerAndroid.AndroidFcmToken = AndroidFcmToken;
                    db.Customers.Update(customerAndroid);
                    db.SaveChanges();
                }
                if (IosFcmToken != "")
                {
                    var customerIOS = db.Customers.Where(x => (x.Email == Email || x.Mobile == Email) && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
                    customerIOS.IosFcmToken = IosFcmToken;
                    db.Customers.Update(customerIOS);
                    db.SaveChanges();
                }
                if (customer != null)
                {

                    var customerUser = (from c in db.CustomerRoles
                                        join d in db.Customers on c.CutomerRoleId equals d.CustomerId
                                        where c.CutomerRoleId == customer.CustomerId && c.RoleName == "Customer"
                                        select new
                                        {
                                            c.RoleName
                                        }
                                    ).FirstOrDefault();
                }
                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<CustomerListDto> AddUpdateCustomer(CustomerEditDto input)
        {
            CustomerListDto customer = new CustomerListDto();
            if (input.CustomerId > 0)
            {
                customer = await UpdateCustomer(input);
            }
            else
            {
                customer = await CreateCustomer(input);
            }
            return (customer);
        }

        protected virtual async Task<CustomerListDto> UpdateCustomer(CustomerEditDto input)
        {
            try
            {
                var dto = await db.Customers.Where(g => g.CustomerId == input.CustomerId).FirstOrDefaultAsync();
                dto.CustomerName = input.CustomerName;
                //dto.Mobile = input.mobile;
                dto.Cnic = input.Cnic;
                //dto.Email = input.Email;
                if (input.ImagePath != "")
                {
                    dto.ProfileImage = input.ImagePath;
                }
                dto.CutomerRoleId = 1;
                dto.ModifiedDate = DateTime.Now;

                db.Customers.Update(dto);
                db.SaveChanges();
                var response = new CustomerListDto
                {
                    CustomerId = dto.CustomerId,
                    CustomerName = dto.CustomerName,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    ProfileImage = dto.ProfileImage,

                };
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        protected virtual async Task<CustomerListDto> CreateCustomer(CustomerEditDto input)
        {
            try
            {


                if (db.Customers.Where(x => x.Email == input.Email || x.Mobile == input.Mobile).Count() == 0)
                {
                    var dto = new Customer
                    {
                        CustomerName = input.CustomerName,
                        Email = input.Email,
                        Password = input.Password,
                        ProfileImage = input.ImagePath,
                        Mobile = input.Mobile,
                        Cnic = input.Cnic,
                        CutomerRoleId = 1,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                    };
                    await db.Customers.AddAsync(dto);
                    await db.SaveChangesAsync();

                    await CustomerWallet(dto.CustomerId);
                    var response = new CustomerListDto
                    {
                        CustomerId = dto.CustomerId,
                        CustomerName = dto.CustomerName,
                        Email = dto.Email,
                        ProfileImage = dto.ProfileImage,
                        Mobile = dto.Mobile,

                    };
                    return response;
                }
                else
                {
                    CustomerListDto response = new CustomerListDto();
                    return response;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<RoomCityWise>> GetAllRoomCityWise(string CityName)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            return await db.Ads.Where(d => d.Location.Contains(CityName) && d.IsDeleted == false)
                 .Select(g => new RoomCityWise
                 {
                     AdId = g.AdId,
                     NoOfBed = g.NoOfBed,
                     NoOfPerson = g.NoOfPerson,
                     AdImage1 = ImagePath + g.AdImage1,
                     Price = g.Price,
                     BasePrice = g.BasePrice,
                     RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault()
                 }).ToListAsync();
        }

        public async Task<List<RoomCityWise>> GetAllRoomList()
        {
            var ImagePath = configuration["LoftyImage:Image"];
            return await db.Ads.Where(d => d.IsDeleted == false)
                 .Select(g => new RoomCityWise
                 {
                     AdId = g.AdId,
                     NoOfBed = g.NoOfBed,
                     NoOfPerson = g.NoOfPerson,
                     AdImage1 = ImagePath + g.AdImage1,
                     Price = g.Price,
                     BasePrice = g.BasePrice,
                     RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault()
                 }).ToListAsync();
        }
        public async Task<List<RoomPackageWise>> GetAllPackageRoomList()
        {
            var ImagePath = configuration["LoftyImage:Image"];
            return await db.Ads.Where(d => d.IsDeleted == false && d.PackageId > 0)
                 .Select(g => new RoomPackageWise
                 {
                     AdId = g.AdId,
                     //NoOfBed = g.NoOfBed,
                     //NoOfPerson = g.NoOfPerson,
                     AdImage1 = ImagePath + g.AdImage1,
                     Price = g.Price,
                     Description = g.Description,
                     PackageStartDate = g.PackageStartDate,
                     PackageEndDate = g.PackageEndDate,
                     PackageName = db.Packages.Where(g => g.IsDeleted == false && g.PackageId == g.PackageId).Select(a => a.PromotionalText).FirstOrDefault(),
                     RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault()
                 }).OrderByDescending(x => x.AdId).ToListAsync();
        }
        public async Task<List<CityListDto>> GetAllCityList()
        {
            return await db.Cities
                 .Select(g => new CityListDto
                 {
                     Id = g.Id,
                     CityName = g.CityName
                 }).ToListAsync();
        }
        public async Task<GetRoomDetail> GetRoomDetail(int AdId, int CustomerId)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            GetRoomDetail getRoomDetail = new GetRoomDetail();
            if (CustomerId == 0)
            {
                getRoomDetail.RoomDetail = await db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).
          Select(g => new RoomDetailDto
          {
              AdId = g.AdId,
              NoOfBed = g.NoOfBed,
              NoOfPerson = g.NoOfPerson,
              Price = g.Price,
              BasePrice = g.BasePrice,
              RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault(),
              Description = g.Description,
              HotelName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.HotelName).FirstOrDefault(),
              Location = g.Location,
              IsFavourite = false,
              Latitude = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.Latitude).FirstOrDefault(),
              Longitude = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.Longitude).FirstOrDefault(),
              PartnerId = (int)g.PartnerId,
          }).FirstOrDefaultAsync();
            }
            else
            {
                getRoomDetail.RoomDetail = await db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).
                          Select(g => new RoomDetailDto
                          {
                              AdId = g.AdId,
                              NoOfBed = g.NoOfBed,
                              NoOfPerson = g.NoOfPerson,
                              Price = g.Price,
                              BasePrice = g.BasePrice,
                              RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                              Description = g.Description,
                              HotelName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.HotelName).FirstOrDefault(),
                              Location = g.Location,
                              IsFavourite = db.FavouriteAds.Where(a => a.IsDeleted == false && a.AdId == AdId && a.CustomerId == CustomerId).Select(a => a.IsFavourite).FirstOrDefault(),
                              Latitude = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.Latitude).FirstOrDefault(),
                              Longitude = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.Longitude).FirstOrDefault(),
                              PartnerId = (int)g.PartnerId,
                          }).FirstOrDefaultAsync();
            }


            var images = db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage1)
     .Concat(db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage2)).Concat(db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false)
     .Select(i => ImagePath + i.AdImage3)).Concat(db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage4))
     .ToList();

            var facl = db.AdFacilities.Where(adFacility => adFacility.AdId == AdId).Join(db.Facilities, adFacility => adFacility.FacilityId, facility => facility.FacilityId, (adFacility, facility) => new
            {
                FacilityName = facility.FacilityName,
                Image = facility.Image,
                Count = facility.Count,
            }).Select(d => new FaciltyDetailList
            {
                FacilityName = d.FacilityName,
                Image = ImagePath + d.Image,
                Count = d.Count,
            }).ToList();

            var checkindate = db.Bookings.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(a => a.CheckedIn).FirstOrDefault();
            var chechkoutdate = db.Bookings.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(a => a.CheckOut).FirstOrDefault();


            var adbookingdatelist = db.Bookings.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(a => new BookingDateDto
            {
                CheckedIn = a.CheckedIn,
                CheckOut = a.CheckOut
            }).ToList();


            getRoomDetail.RoomImages = images;
            List<DateTime> datesList1 = new List<DateTime>();
            foreach (var item in adbookingdatelist)
            {
                var datesBetween1 = Enumerable.Range(0, (item.CheckOut - item.CheckedIn).Days + 1)
                                            .Select(offset => item.CheckedIn.AddDays(offset));
                datesList1.AddRange(datesBetween1);
            }

            //var datesBetween = Enumerable.Range(0, (chechkoutdate - checkindate).Days + 1)
            //.Select(offset => checkindate.AddDays(offset));


            //List<DateTime> datesList = Enumerable.Range(0, (chechkoutdate - checkindate).Days + 1)
            //                            .Select(offset => checkindate.AddDays(offset))
            //                            .ToList();
            getRoomDetail.ReservedDates = datesList1;
            getRoomDetail.FacilityList = facl;
            return getRoomDetail;
        }
        public async Task<CustomerPasswordDto> GetCustomerById(int CustomerId)
        {
            CustomerPasswordDto getCustomer = new CustomerPasswordDto();

            getCustomer = await db.Customers.Where(g => g.CustomerId == CustomerId && g.IsDeleted == false).
            Select(g => new CustomerPasswordDto
            {
                CustomerId = g.CustomerId,
                CustomerName = g.CustomerName,
                Email = g.Email,
                Password = g.Password,
            }).FirstOrDefaultAsync();
            return getCustomer;
        }

        public async Task UpdatePassword(string NewPassword, int CustomerId)
        {
            var customer = db.Customers.Where(g => g.CustomerId == CustomerId).FirstOrDefault();

            if (customer != null)
            {
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    customer.Password = NewPassword;
                }
                await db.SaveChangesAsync();
            }
        }


        public async Task AddToFavourite(int AdId, bool IsFavourite, int CustomerId)
        {
            var fav = db.FavouriteAds.Where(g => g.AdId == AdId).FirstOrDefault();

            if (fav != null)
            {
                fav.IsFavourite = IsFavourite;
                await db.SaveChangesAsync();
            }
            else
            {
                var dto = new FavouriteAd
                {
                    CustomerId = CustomerId,
                    AdId = AdId,
                    IsFavourite = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                };
                await db.FavouriteAds.AddAsync(dto);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<RoomCityWise>> GetAllFavouriteRoom(int CustomerId)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            return await db.FavouriteAds.Where(d => d.IsDeleted == false && d.IsFavourite == true && d.CustomerId == CustomerId)
                 .Select(g => new RoomCityWise
                 {
                     AdId = g.AdId,
                     //NoOfBed = g.NoOfBed,
                     //NoOfPerson = g.NoOfPerson,
                     AdImage1 = ImagePath + g.Ads.AdImage1,
                     Price = g.Ads.Price,
                     RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault()
                 }).ToListAsync();
        }

        public async Task<List<NearByRomms>> GetAllNearByRooms(double latitude, double longitude, double radius)
        {
            double radiusInKm;
            if (radius != 0)
            {
                radiusInKm = radius;
                // The value is not null, do something with radius.Value
            }
            else
            {
                radiusInKm = 25.0;
            }

            //const double radiusInKm = 25.0;
            // Calculate the boundaries of the radius
            double earthRadius = 6371; // Earth's radius in kilometers
            double lat = Math.PI * latitude / 180.0;
            double lon = Math.PI * longitude / 180.0;
            double distance = radiusInKm / earthRadius;

            double minLat = latitude - (distance * 180.0 / Math.PI);
            double maxLat = latitude + (distance * 180.0 / Math.PI);
            double minLon = longitude - distance / Math.Cos(lat) * 180.0 / Math.PI;
            double maxLon = longitude + distance / Math.Cos(lat) * 180.0 / Math.PI;
            var ImagePath = configuration["LoftyImage:Image"];
            return await db.Ads.Where(d => d.Partners.Latitude >= minLat && d.Partners.Latitude <= maxLat &&
                            d.Partners.Longitude >= minLon && d.Partners.Longitude <= maxLon && d.IsDeleted == false)
                 .Select(g => new NearByRomms
                 {
                     AdId = g.AdId,
                     NoOfPerson = g.NoOfPerson,
                     NoOfBed = g.NoOfBed,
                     AdImage1 = ImagePath + g.AdImage1,
                     Price = g.Price,
                     RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault()
                 }).ToListAsync();
        }

        public async Task<List<RoomCityWise>> GetAllFilterRoomList(FilterRoomListDto input)
        {
            var ImagePath = configuration["LoftyImage:Image"];

            var list = db.Ads
                .Where(d => d.IsDeleted == false)
                .Where(d => (d.Price >= input.MinPrice && d.Price <= input.MaxPrice) || input.MaxPrice == 0)
                .Where(d => !db.Bookings.Any(b => b.AdId == d.AdId && (b.CheckedIn <= input.CheckOutDate && b.CheckOut >= input.CheckedInDate)))
                .Where(d => d.Rooms.RoomType.ToLower().Contains(input.RoomType))
                    .Select(g => new RoomCityWise
                    {
                        AdId = g.AdId,
                        NoOfBed = g.NoOfBed,
                        NoOfPerson = g.NoOfPerson,
                        AdImage1 = ImagePath + g.AdImage1,
                        Price = g.Price,
                        RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault()
                    }).AsEnumerable().Skip(input.PageNumber * input.Rows).Take(input.Rows).ToList();
            return list;

        }


        public static string GenerateVoucherNumber()
        {
            var chars = "0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result.ToString();
        }


        public async Task<GetCustomerWalletOutPut> CustomerWallet(int CustomerId)
        {
            var dto = new CustomerWalletAmount
            {
                CustomerId = CustomerId,
                WalletAnount = 0,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.CustomerWalletAmounts.Add(dto);
            db.SaveChanges();

            return new GetCustomerWalletOutPut { CustomerId = dto.CustomerId };
        }


        public async Task<GetCustomerWalletOutPut> GetVoucherCode(int CustomerId)
        {
            var dto = new CustomerWallet
            {
                VoucherNumber = GenerateVoucherNumber(),
                VoucherDate = DateTime.Now,
                CustomerId = CustomerId,
                WalletAnount = 0,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.CustomerWallets.Add(dto);
            db.SaveChanges();

            return new GetCustomerWalletOutPut { CustomerId = dto.CustomerId, VoucherNumber = dto.VoucherNumber };
        }



        public async Task<VoucherDto> GetVoucherTotalAmount(int CustomerId)
        {
            VoucherDto customerVouvher = new VoucherDto();
            customerVouvher.TotalAmount = await db.CustomerWalletAmounts
            .Where(g => g.CustomerId == CustomerId && g.IsDeleted == false)
            .SumAsync(g => g.WalletAnount);

            return customerVouvher;
        }

        public async Task<VoucherListDto> AddWalletAmount(VoucherAmountDto input)
        {
            VoucherListDto amountDto = new VoucherListDto();
            if (input.CustomerId > 0)
            {
                amountDto = await UpdateNewAmount(input);
            }

            return (amountDto);
        }

        public async Task<GetCustomerUnVerifiedAmount> AddVoucherAmount(VouchePriceDto input)
        {
            var dto = new CustomerWallet
            {
                VoucherNumber = GenerateVoucherNumber(),
                VoucherDate = DateTime.Now,
                CustomerId = input.CustomerId,
                WalletAnount = 0,
                UnverifiedAmount = input.UnVerifiedAmount,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.CustomerWallets.Add(dto);
            db.SaveChanges();

            return new GetCustomerUnVerifiedAmount { UnverifiedAmount = dto.UnverifiedAmount, VoucherNumber = dto.VoucherNumber };
        }

        protected virtual async Task<VoucherListDto> UpdateNewAmount(VoucherAmountDto input)
        {
            try
            {
                var dto = await db.CustomerWallets.Where(g => g.CustomerId == input.CustomerId && g.VoucherNumber == input.VoucherNumber).FirstOrDefaultAsync();
                var customerwalletamount = await db.CustomerWalletAmounts.Where(g => g.CustomerId == input.CustomerId).FirstOrDefaultAsync();
                if (dto.UnverifiedAmount != 0)
                {
                    customerwalletamount.WalletAnount += dto.UnverifiedAmount;
                    dto.UnverifiedAmount = 0;
                }


                //dto.WalletAnount = dto.UnverifiedAmount;

                dto.ModifiedDate = DateTime.Now;

                db.CustomerWallets.Update(dto);
                db.SaveChanges();

                db.CustomerWalletAmounts.Update(customerwalletamount);
                db.SaveChanges();

                var response = new VoucherListDto
                {
                    CustomerId = dto.CustomerId,
                    VoucherNumber = dto.VoucherNumber,
                    Amount = customerwalletamount.WalletAnount,
                };
                return response;




            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<List<CustomerWalletHistoryDto>> CustomerWalletHistory(int CustomerId)
        {

            var list = db.CustomerWalletHistory
                .Where(d => d.IsDeleted == false && d.CustomerId == CustomerId).Select(g => new CustomerWalletHistoryDto
                {
                    CustomerName = db.Customers.Where(g => g.CustomerId == CustomerId).Select(a => a.CustomerName).FirstOrDefault(),
                    Type = g.Type,
                    Amount = g.Amount,
                }).ToList();
            return list;
        }


        public async Task<bool> CustomerHelpSupport(int CustomerId, string Message)
        {
            var dto = new CustomerHelpSupport
            {
                CustomerId = CustomerId,
                Message = Message,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.CustomerHelpSupports.Add(dto);
            db.SaveChanges();

            return true;
        }


        public async Task<bool> AddCustomerRatingAndReview(int CustomerId, CustomerRatingDto input)
        {
            var dto = new CustomerRatingAndReview
            {
                CustomerId = CustomerId,
                Rating = input.Rating,
                Review = input.Review,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.CustomerRatingAndReviews.Add(dto);
            db.SaveChanges();

            return true;
        }
        public static string GenerateWalletRefNumber()
        {
            var chars = "0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result.ToString();
        }
        public async Task<WalletListDto> GetCustomerWalletAmountByCustomerId(int CustomerId)
        {
            WalletListDto getwallet = new WalletListDto();

            getwallet = await db.CustomerWalletAmounts.Where(g => g.CustomerId == CustomerId && g.IsDeleted == false).
            Select(g => new WalletListDto
            {
                CustomerId = g.CustomerId,
                WalletId = g.Id,
                Amount = Convert.ToInt32(g.WalletAnount),
                RefNumber = GenerateWalletRefNumber()
            }).FirstOrDefaultAsync();
            return getwallet;
        }
        public async Task DeleteCustomerData(int CustomerId)
        {
            var customer = db.Customers.Where(g => g.CustomerId == CustomerId).FirstOrDefault();
            customer.IsDeleted = true;
            customer.ModifiedDate = DateTime.Now;
            db.Customers.Update(customer);
            db.SaveChanges();
        }


        public async Task<WalletAmountDto> AddCustomerWalletAmount(int CustomerId, decimal Price)
        {
            try
            {
                WalletAmountDto amountdto = new WalletAmountDto();
                var dto = await db.CustomerWalletAmounts.Where(g => g.CustomerId == CustomerId).FirstOrDefaultAsync();
                //var customerwalletamount = await db.CustomerWalletAmounts.Where(g => g.CustomerId == input.CustomerId).FirstOrDefaultAsync();
                if (dto != null)
                {
                    dto.WalletAnount += Price;
                    dto.ModifiedDate = DateTime.Now;
                    db.CustomerWalletAmounts.Update(dto);
                    db.SaveChanges();


                    var response = new WalletAmountDto
                    {
                        CustomerId = dto.CustomerId,
                        Amount = dto.WalletAnount,
                    };
                    return response;
                }
                else
                {
                    return amountdto;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<bool> AddCustomerWalletHistor(int CustomerID, decimal Price)
        {
            var dto = new CustomerWalletHistory
            {
                CustomerId = CustomerID,
                Amount = Price,
                Type = WalletType.Credit,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            };

            db.CustomerWalletHistory.Add(dto);
            db.SaveChanges();

            return true;

        }


        public async Task<bool> AddCustomerWalletHistorWithDebit(int CustomerID, decimal Price)
        {
            var dto = new CustomerWalletHistory
            {
                CustomerId = CustomerID,
                Amount = Price,
                Type = WalletType.debit,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            };

            db.CustomerWalletHistory.Add(dto);
            db.SaveChanges();

            return true;

        }
        public async Task<CustomerExist> IsCustomerAlreadyExist(string Email, string Mobile)
        {

            var message = "";
            if (db.Customers.Where(a => a.IsDeleted == false && a.Email == Email).Count() > 0 || db.Partners.Where(a => a.IsDeleted == false && a.Email == Email).Count() > 0)
            {
                message = "Email Already Exist!";
            }
            if (db.Customers.Where(a => a.IsDeleted == false && a.Mobile == Mobile).Count() > 0 || db.Partners.Where(a => a.IsDeleted == false && a.Phone == Mobile).Count() > 0)
            {
                message = "Mobile Already Exist!";
            }
            if (db.Customers.Where(a => a.IsDeleted == false && a.Email == Email && a.Mobile == Mobile).Count() > 0 || db.Partners.Where(a => a.IsDeleted == false && a.Email == Email && a.Phone == Mobile).Count() > 0)
            {
                message = "Email & Mobile Already Exist!";
            }

            //var customer = db.Customers.Where(g => g.IsDeleted == false && g.Email == Email || g.Mobile == Mobile).FirstOrDefault();
            if (message == "")
            {
                var res = new CustomerExist
                {
                    IsExist = true,
                    Message = "Email or Phone No is Not Exist!",
                };
                return res;
            }
            else
            {
                var res = new CustomerExist
                {
                    IsExist = false,
                    Message = message,
                };
                return res;
            }
        }


        public async Task CustomerWalletPayitHistory(int CustomerId, string ReferenceNumber, decimal Amount)
        {
            var dto = new CustomerWalletHistoryForPayit
            {
                CustomerId = CustomerId,
                ReferenceNumber = ReferenceNumber,
                Amount = Amount,
                Status = PayitStatus.UnPaid,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            };
            await db.AddAsync(dto);
            await db.SaveChangesAsync();
        }

        public async Task UpdateCustomerWalletStatus(string ReferenceNumber)
        {
            try
            {
                var dto = await db.CustomerWalletHistoryForPayit.Where(g => g.IsDeleted == false && g.ReferenceNumber == ReferenceNumber).FirstOrDefaultAsync();
                if (dto != null)
                {
                    dto.Status = PayitStatus.Paid;
                    dto.ModifiedDate = DateTime.Now;
                    db.CustomerWalletHistoryForPayit.Update(dto);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public async Task<CustomerWalletPaymentAndHistory> CustomerWalletPayitStatus(int CustomerId)
        {
            CustomerWalletPaymentAndHistory customerwallet = new CustomerWalletPaymentAndHistory();

            customerwallet.DepositHistoy = db.CustomerWalletHistoryForPayit
                .Where(d => d.IsDeleted == false && d.CustomerId == CustomerId).Select(g => new PayitWalletHistoryListDto
                {
                    CustomerName = db.Customers.Where(g => g.CustomerId == CustomerId).Select(a => a.CustomerName).FirstOrDefault(),
                    Status = g.Status,
                    Amount = g.Amount,
                    ReferenceNumber = g.ReferenceNumber,
                    CreatedDate = g.CreatedDate,
                }).ToList();
            customerwallet.WalletHistory = db.CustomerWalletHistory
                .Where(d => d.IsDeleted == false && d.CustomerId == CustomerId).Select(g => new CustomerWalletHistoryDto
                {
                    CustomerName = db.Customers.Where(g => g.CustomerId == CustomerId).Select(a => a.CustomerName).FirstOrDefault(),
                    Type = g.Type,
                    Amount = g.Amount,
                    CreatedDate = g.CreatedDate,
                }).ToList();
            return customerwallet;
        }

        public async Task<List<CustomerHelpAndSupportMessageListDto>> CustomerHelpAndSupportMessage()
        {

            var list = db.CustomerHelpSupports
                .Where(d => d.IsDeleted == false).Select(g => new CustomerHelpAndSupportMessageListDto
                {
                    CustomerName = db.Customers.Where(a => a.IsDeleted == false && a.CustomerId == g.CustomerId).Select(a => a.CustomerName).FirstOrDefault(),
                    Email = db.Customers.Where(a => a.IsDeleted == false && a.CustomerId == g.CustomerId).Select(a => a.Email).FirstOrDefault(),
                    Mobile = db.Customers.Where(a => a.IsDeleted == false && a.CustomerId == g.CustomerId).Select(a => a.Mobile).FirstOrDefault(),
                    Message = g.Message,
                    CreatedDate = g.CreatedDate
                }).ToList();
            return list;
        }

        public async Task<bool> AddCustomerFirBaseNotification(string Title, string Body)
        {
            var dto = new CustomerFireBaseNotification
            {
                Title = Title,
                Body = Body,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.CustomerFireBaseNotifications.Add(dto);
            db.SaveChanges();
            return true;
        }
        public async Task<List<FireBaseNotificationListDto>> GetAllCustomerFireBaseNotification()
        {
            return await db.CustomerFireBaseNotifications.Where(d => d.IsDeleted == false)
                 .Select(g => new FireBaseNotificationListDto
                 {
                     NotificationId = g.NotificationId,
                     Title = g.Title,
                     Body = g.Body,
                     CreatedDate = g.CreatedDate,
                 }).ToListAsync();
        }
        public async Task<List<CustomerListWithTokens>> GetAllCustomerWithToken()
        {
            return await db.Customers.Where(d => d.IsDeleted == false)
                 .Select(g => new CustomerListWithTokens
                 {
                     CustomerName = g.CustomerName,
                     Email = g.Email,
                     Mobile = g.Mobile,
                     AndroidFcmToken = g.AndroidFcmToken,
                     IosFcmToken = g.IosFcmToken
                 }).ToListAsync();
        }
        public bool SendNotification(string Token, string Body, string Title)
        {
            try
            {
                var applicationID = "AAAA2T-9K24:APA91bGq7w95aqg356mY1FL2iIS7eOwOVy4wafRRbnkklc9ytdivCJETCq3y01FaP46Mpnln3Vpcg3tcZWbwCyM_QtwrO_hYSNA5FR-K0NZbeftYa65DGuZic8H0Mhm9q_QvYRLVicXC";
                var senderId = "933077265262";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = Token,
                    notification = new
                    {
                        body = Body,
                        title = Title,
                    },
                    priority = "high"
                };
                string json = JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            Console.WriteLine(sResponseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
