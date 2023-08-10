using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Administration;
using LoftyRoomsModel.Bookings;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.Partners;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsDAL.Booking
{
    public class BookingDAL
    {
        ApplicationDBContext db;
        private readonly IConfiguration configuration;
        public BookingDAL(ApplicationDBContext _db, IConfiguration _configuration)
        {
            configuration = _configuration;
            db = _db;
        }
        public async Task<GetBookRoomDetail> BookRoom(int AdId)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            GetBookRoomDetail getRoomDetail = new GetBookRoomDetail();
            getRoomDetail.RoomDetail = await db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).
                Select(g => new BookRoomDetail
                {
                    AdId = g.AdId,
                    Price = g.Price,
                    RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                }).FirstOrDefaultAsync();

            var images = db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage1).ToList();
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

            getRoomDetail.RoomImages = images;

            getRoomDetail.FacilityList = facl;
            return getRoomDetail;
        }

        public async Task<GetBookRoomDetail> BookRoomDetail(int AdId)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            GetBookRoomDetail getRoomDetail = new GetBookRoomDetail();
            getRoomDetail.RoomDetail = await db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).
                Select(g => new BookRoomDetail
                {
                    AdId = g.AdId,
                    Price = g.Price,
                    RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                    HotelName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.HotelName).FirstOrDefault(),
                    Location = g.Location,


                }).FirstOrDefaultAsync();

            var images = db.Ads.Where(g => g.AdId == AdId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage1).ToList();
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

            getRoomDetail.RoomImages = images;

            getRoomDetail.FacilityList = facl;
            return getRoomDetail;
        }


        public async Task<BookingListDto> GetAllBookings(int CustomerId)
        {

            BookingListDto bookingList = new BookingListDto();
            var ImagePath = configuration["LoftyImage:Image"];
            var active = await db.Bookings.Where(d => d.IsDeleted == false && d.Status == BookingStatus.Active && d.CustomerId == CustomerId)
      .Select(g => new BookingDto
      {
          AdId = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.AdId).FirstOrDefault(),
          AdImage1 = db.Ads.Where(c => c.AdId == g.AdId).Select(g => ImagePath + g.AdImage1).FirstOrDefault(),
          NoOfBed = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfBed).FirstOrDefault(),
          Price = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.Price).FirstOrDefault(),
          RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault(),
          CheckedIn = g.CheckedIn,
          CheckOut = g.CheckOut,
          BookingId = g.BookingId,
          BookingNumber = db.Bookings.Where(b => b.IsDeleted == false && b.Status == BookingStatus.Active && b.AdId == g.Ads.AdId).Select(q => q.BookingNumber).FirstOrDefault(),
          PaidStatus = (int)g.PaidStatus,
          BookingDate = g.BookingDate,
          Status = (int)g.Status,
          IsRated = g.IsRated,
      }).ToListAsync();

            var Booked = await db.Bookings.Where(d => d.IsDeleted == false && d.Status == BookingStatus.Booked && d.CustomerId == CustomerId)
        .Select(g => new BookingDto
        {
            AdId = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.AdId).FirstOrDefault(),
            AdImage1 = db.Ads.Where(c => c.AdId == g.AdId).Select(g => ImagePath + g.AdImage1).FirstOrDefault(),
            NoOfBed = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfBed).FirstOrDefault(),
            NoOfPerson = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfPerson).FirstOrDefault(),
            Price = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.Price).FirstOrDefault(),
            RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault(),
            CheckedIn = g.CheckedIn,
            CheckOut = g.CheckOut,
            BookingId = g.BookingId,
            BookingNumber = db.Bookings.Where(b => b.IsDeleted == false && b.Status == BookingStatus.Booked && b.AdId == g.Ads.AdId).Select(q => q.BookingNumber).FirstOrDefault(),
            PaidStatus = (int)g.PaidStatus,
            BookingDate = g.BookingDate,
            Status = (int)g.Status,
            IsRated = g.IsRated,
        }).ToListAsync();

            var Past = await db.Bookings.Where(d => d.IsDeleted == false && d.CheckedIn < DateTime.Now.Date && d.CheckOut < DateTime.Now.Date && d.CustomerId == CustomerId)
                .Select(g => new BookingDto
                {
                    AdId = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.AdId).FirstOrDefault(),
                    AdImage1 = db.Ads.Where(c => c.AdId == g.AdId).Select(g => ImagePath + g.AdImage1).FirstOrDefault(),
                    NoOfBed = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfBed).FirstOrDefault(),
                    NoOfPerson = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfPerson).FirstOrDefault(),
                    Price = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.Price).FirstOrDefault(),
                    CheckedIn = g.CheckedIn,
                    CheckOut = g.CheckOut,
                    BookingId = g.BookingId,
                    RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                    BookingNumber = db.Bookings.Where(b => b.IsDeleted == false && b.Status == BookingStatus.Past && b.AdId == g.Ads.AdId).Select(q => q.BookingNumber).FirstOrDefault(),
                    PaidStatus = (int)g.PaidStatus,
                    BookingDate = g.BookingDate,
                    Status = (int)g.Status,
                    IsRated = g.IsRated,
                }).ToListAsync();
            bookingList.ActiveBookings = active;
            bookingList.BookedBookings = Booked;
            bookingList.PastBookings = Past;

            return bookingList;
        }

        public static string GetRandomNumberNumeric()
        {
            var chars = "0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result.ToString();
        }
        public async Task<GetBookingOutPut> CreateBookingOld(CreateBookingInput input)
        {
            var dto = new LoftyRoomsModel.Bookings.Booking
            {
                BookingNumber = GetRandomNumberNumeric(),
                BookingDate = DateTime.Now,
                AdId = input.AdId,
                CustomerId = input.CustomerId,
                Price = input.Price,
                CheckedIn = input.CheckedIn,
                CheckOut = input.CheckOut,
                Status = (BookingStatus)input.Status,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.Bookings.Add(dto);
            db.SaveChanges();

            return new GetBookingOutPut { BookingId = dto.BookingId, BookingNumber = dto.BookingNumber, BookingDate = dto.BookingDate, Price = dto.Price, CustomerId = dto.CustomerId };
        }


        public async Task<GetBookingOutPut> CreateBooking(CreateBookingInput input)
        {
            var dto = new LoftyRoomsModel.Bookings.Booking
            {
                BookingNumber = GetRandomNumberNumeric(),
                BookingDate = DateTime.Now,
                AdId = input.AdId,
                CustomerId = input.CustomerId,
                Price = input.Price,
                CommissionPrice = input.AdminCommissionPrice,
                //Paid = false,
                CheckedIn = input.CheckedIn,
                CheckOut = input.CheckOut,
                Status = BookingStatus.Active,
                PaidStatus = CheckPaid.None,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };
            db.Bookings.Add(dto);
            db.SaveChanges();
            var PartnerId = db.Bookings.Where(a => a.IsDeleted == false && a.BookingId == dto.BookingId).Select(b => b.Ads.PartnerId).FirstOrDefault();
            return new GetBookingOutPut { BookingId = dto.BookingId, BookingNumber = dto.BookingNumber, BookingDate = dto.BookingDate, Price = dto.Price, CustomerId = dto.CustomerId, PartnerId = (int)PartnerId };
        }
        public async Task AddBookingNotification(BookingNotificationDto dto)
        {
            var notificaiton = new BookingNotification
            {
                CustomerId = dto.CustomerId,
                Message = dto.Message,
                Status = dto.Status,
                CreatedDate = DateTime.Now
            };
            await db.AddAsync(notificaiton);
            await db.SaveChangesAsync();
        }


        public async Task<List<NotificationListDto>> GetAllNotification(int CustomerId)
        {
            return await db.BookingNotifications.Where(k => k.IsDeleted == false && k.CustomerId == CustomerId)
                 .Select(g => new NotificationListDto
                 {
                     Message = g.Message,
                     Status = g.Status,
                 }).ToListAsync();
        }
        public async Task<bool> PaymentThroughWallet(int CustomerId, decimal Price, decimal CommissionPrice, int AdId)
        {
            var PartnerPrice = Price - CommissionPrice;
            var dto = await db.CustomerWalletAmounts.Where(g => g.CustomerId == CustomerId).FirstOrDefaultAsync();

            var partnerId = await db.Ads.Where(a => a.AdId == AdId).Select(p => p.PartnerId).FirstOrDefaultAsync();
            var partnerwalletamount = await db.PartnerWalletAmounts.Where(g => g.PartnerId == partnerId).FirstOrDefaultAsync();
            var adminwalletamount = await db.AdminWalletAmounts.FirstOrDefaultAsync();

            if (dto.WalletAnount >= Price)
            {
                dto.WalletAnount = dto.WalletAnount - Price;
                db.CustomerWalletAmounts.Update(dto);
                db.SaveChanges();

                partnerwalletamount.WalletAnount += PartnerPrice;
                db.PartnerWalletAmounts.Update(partnerwalletamount);
                db.SaveChanges();

                adminwalletamount.WalletAnount += CommissionPrice;
                adminwalletamount.ModifiedDate = DateTime.Now;
                db.AdminWalletAmounts.Update(adminwalletamount);
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GetInquiryOutPut> AddInquiry(int BookingId, string BookingNumber, DateTime BookingDate, decimal Price)
        {
            DateTime dateOnly = BookingDate.Date;
            string dateone = BookingDate.Date.ToString();
            var yearAndMonthDay = BookingDate.ToString("yyyy-MM-dd");
            var yearAndMonth = BookingDate.ToString("yyyy-MM");
            var Key = configuration["PayitApiKey:Key"];
            var bankcodedefault = configuration["PayitApiKey:BankAccountCode"];
            var xapikeydefault = configuration["PayitApiKey:X_API_KEY"];
            var dto = new Inquiry
            {
                BookingId = BookingId,
                //Here CustomerWalletAmountsId is WalletID
                //CustomerWalletAmountsId = 0,
                Amount = Price,
                LateAmount = Price,
                YearMonthFrom = yearAndMonth,
                YearMonthTo = yearAndMonth,
                Description = "Payit",
                DueDate = yearAndMonthDay,
                VoucherValidTillDate = yearAndMonthDay,
                StudentIdentificationNumber = "Na",
                ClassName = "Na",
                SectionName = "Na",
                InstituteName = "Na",
                SessionName = "Na",
                BankAccountCode = bankcodedefault,
                CNIC = "Na",
                StudentName = "Na",
                MobileNumber = "Na",
                Email = "Na",
                ReferenceNumber = Key + BookingNumber,
                X_API_KEY = xapikeydefault,
                //ReturnValue="0"
            };

            db.Inquiries.Add(dto);
            db.SaveChanges();

            var res = new GetInquiryOutPut
            {
                BookingId = BookingId,
                ReferenceNumber = dto.ReferenceNumber,
                BankAccountCode = dto.BankAccountCode,
                X_API_KEY = dto.X_API_KEY,
            };

            return res;
        }


        public async Task<GetInquiryOutPut> AddInquiryForWallet(int WalletId, string RefNumber, DateTime Walletdate, decimal Amount)
        {
            DateTime dateOnly = Walletdate.Date;
            string dateone = Walletdate.Date.ToString();
            var yearAndMonthDay = Walletdate.ToString("yyyy-MM-dd");
            var yearAndMonth = Walletdate.ToString("yyyy-MM");
            var Key = configuration["PayitApiKey:Key"];
            var bankcodedefault = configuration["PayitApiKey:BankAccountCode"];
            var xapikeydefault = configuration["PayitApiKey:X_API_KEY"];
            var dto = new Inquiry
            {
                //BookingId = 0,
                //here CustomerWalletAmountsId is Wallet id
                CustomerWalletAmountsId = WalletId,
                Amount = Amount,
                LateAmount = Amount,
                YearMonthFrom = yearAndMonth,
                YearMonthTo = yearAndMonth,
                Description = "Wallet",
                DueDate = yearAndMonthDay,
                VoucherValidTillDate = yearAndMonthDay,
                StudentIdentificationNumber = "Na",
                ClassName = "Na",
                SectionName = "Na",
                InstituteName = "Na",
                SessionName = "Na",
                BankAccountCode = bankcodedefault,
                CNIC = "Na",
                StudentName = "Na",
                MobileNumber = "Na",
                Email = "Na",
                ReferenceNumber = Key + RefNumber,
                X_API_KEY = xapikeydefault,
            };

            db.Inquiries.Add(dto);
            db.SaveChanges();

            var res = new GetInquiryOutPut
            {
                ReferenceNumber = dto.ReferenceNumber,
                BankAccountCode = dto.BankAccountCode,
                X_API_KEY = dto.X_API_KEY,
            };

            return res;
        }

        public async Task<GetInquiryData> GetInquiryData(InquiryFetchDto input)
        {
            GetInquiryData inquirydata = new GetInquiryData();
            input.X_API_KEY = configuration["PayitApiKey:X_API_KEY"];
            input.BankAccountCode = configuration["PayitApiKey:BankAccountCode"];
            inquirydata = await db.Inquiries.Where(g => g.ReferenceNumber == input.ReferenceNumber && g.BankAccountCode == input.BankAccountCode && g.X_API_KEY == input.X_API_KEY).
             Select(g => new GetInquiryData
             {
                 Amount = (int)g.Amount,
                 LateAmount = (int)g.LateAmount,
                 YearMonthFrom = g.YearMonthFrom,
                 YearMonthTo = g.YearMonthTo,
                 Description = g.Description,
                 DueDate = g.DueDate,
                 VoucherValidTillDate = g.VoucherValidTillDate,
                 StudentIdentificationNumber = g.StudentIdentificationNumber,
                 ClassName = g.ClassName,
                 SectionName = g.SectionName,
                 InstituteName = g.InstituteName,
                 SessionName = g.SessionName,
                 BankAccountCode = g.BankAccountCode,
                 CNIC = g.CNIC,
                 StudentName = g.StudentName,
                 MobileNumber = g.MobileNumber,
                 Email = g.Email,
                 ReferenceNumber = g.ReferenceNumber,
                 X_API_KEY = g.X_API_KEY,
                 ReturnValue = "0",
             }).FirstOrDefaultAsync();
            return inquirydata;
        }

        public async Task<DashBoardDto> GetDashBoardData()
        {
            DashBoardDto dashBoardOutPutDto = new DashBoardDto();

            var Activedata = await db.Bookings.Where(g => g.IsDeleted == false && g.Status == BookingStatus.Active).Select(g => new
            {
                BookingDate = g.BookingDate,
                BookingNumber = g.BookingNumber
            }).ToListAsync();

            var Bookeddata = await db.Bookings.Where(g => g.IsDeleted == false && g.Status == BookingStatus.Booked).Select(g => new
            {
                BookingDate = g.BookingDate,
                BookingNumber = g.BookingNumber
            }).ToListAsync();

            var Pastdata = await db.Bookings.Where(g => g.IsDeleted == false && g.CheckedIn < DateTime.Now.Date && g.CheckOut < DateTime.Now.Date).Select(g => new
            {
                BookingDate = g.BookingDate,
                BookingNumber = g.BookingNumber
            }).ToListAsync();

            var Rejectdata = await db.Bookings.Where(g => g.IsDeleted == false && g.Status == BookingStatus.Rejected).Select(g => new
            {
                BookingDate = g.BookingDate,
                BookingNumber = g.BookingNumber
            }).ToListAsync();

            var partnersWithRejections11 = db.Bookings.Where(g => g.Status == BookingStatus.Rejected && g.IsDeleted == false).GroupBy(b => new { b.CustomerId, b.AdId })
                                           .Where(group => group.Count() >= 2).Select(group => new RejectedBookingInfo
                                           {
                                               AdId = (int)group.Key.AdId,
                                               CustomerId = group.Key.CustomerId,
                                               RejectionCount = group.Count()
                                           }).ToList();
            List<RejectedBookingListDto> records = new List<RejectedBookingListDto>();

            RejectedBookingListDto obj = null;
            foreach (var item in partnersWithRejections11)
            {
                obj = new RejectedBookingListDto();

                obj.RejectionCount = item.RejectionCount;
                obj.CustomerName = db.Customers.Where(a => a.IsDeleted == false && a.CustomerId == item.CustomerId).Select(b => b.CustomerName).FirstOrDefault();
                obj.PartnerName = db.Ads.Where(a => a.IsDeleted == false && a.AdId == item.AdId).Select(b => b.Partners.FirstName).FirstOrDefault();
                records.Add(obj);
            }

            var list = await db.Bookings.Where(d => d.IsDeleted == false)
      .Select(g => new AdminBookingDto
      {
          AdId = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.AdId).FirstOrDefault(),
          Price = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.Price).FirstOrDefault(),
          RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault(),
          CheckedIn = g.CheckedIn,
          CheckOut = g.CheckOut,
          BookingId = g.BookingId,
          BookingNumber = db.Bookings.Where(b => b.IsDeleted == false).Select(q => q.BookingNumber).FirstOrDefault(),
          PaidStatus = (int)g.PaidStatus,
          BookingDate = g.BookingDate,
          Status = (int)g.Status,
          CustomerName = db.Customers.Where(c => c.IsDeleted == false && c.CustomerId == g.CustomerId).Select(g => g.CustomerName).FirstOrDefault(),
          PartnerName = db.Partners.Where(c => c.IsDeleted == false && c.PartnerId == g.Ads.PartnerId).Select(g => g.FirstName + " " + g.LastName).FirstOrDefault(),
          HotelName = db.Partners.Where(c => c.IsDeleted == false && c.PartnerId == g.Ads.PartnerId).Select(g => g.HotelName).FirstOrDefault(),
      }).ToListAsync();

            dashBoardOutPutDto.ActiveCount = Activedata.Count();

            dashBoardOutPutDto.BookedCount = Bookeddata.Count();

            dashBoardOutPutDto.PastCount = Pastdata.Count();

            dashBoardOutPutDto.RejectCount = Rejectdata.Count();

            dashBoardOutPutDto.TotalBooking = list;

            dashBoardOutPutDto.CustomerRejectedBooking = records;
            //Pie Chart 
            dashBoardOutPutDto.PieChartData = new List<PieChartDto>();
            dashBoardOutPutDto.PieChartData.Add(new PieChartDto
            {
                Label = "Active",
                Count = db.Bookings.Where(g => g.IsDeleted == false && g.Status == BookingStatus.Active).Count()
            });
            dashBoardOutPutDto.PieChartData.Add(new PieChartDto
            {
                Label = "Booked",
                Count = db.Bookings.Where(g => g.IsDeleted == false && g.Status == BookingStatus.Booked).Count()
            });
            dashBoardOutPutDto.PieChartData.Add(new PieChartDto
            {
                Label = "Past",
                Count = db.Bookings.Where(g => g.IsDeleted == false && g.CheckedIn < DateTime.Now.Date && g.CheckOut < DateTime.Now.Date).Count()
            });
            dashBoardOutPutDto.PieChartData.Add(new PieChartDto
            {
                Label = "Rejected",
                Count = db.Bookings.Where(g => g.IsDeleted == false && g.Status == BookingStatus.Rejected).Count()
            });

            return dashBoardOutPutDto;
        }
        public async Task<BookingListDto> GetAllPartnerBookings(int PartnerId)
        {

            BookingListDto bookingList = new BookingListDto();
            var ImagePath = configuration["LoftyImage:Image"];
            var active = await db.Bookings.Where(d => d.IsDeleted == false && d.Status == BookingStatus.Active && d.Ads.PartnerId == PartnerId)
      .Select(g => new BookingDto
      {
          BookingId = g.BookingId,
          AdId = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.AdId).FirstOrDefault(),
          AdImage1 = db.Ads.Where(c => c.AdId == g.AdId).Select(g => ImagePath + g.AdImage1).FirstOrDefault(),
          NoOfBed = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfBed).FirstOrDefault(),
          Price = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.Price).FirstOrDefault(),
          RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault(),
          CheckedIn = g.CheckedIn,
          CheckOut = g.CheckOut,
          BookingNumber = db.Bookings.Where(b => b.IsDeleted == false && b.Status == BookingStatus.Active && b.AdId == g.Ads.AdId).Select(q => q.BookingNumber).FirstOrDefault(),
          PaidStatus = (int)g.PaidStatus,
          BookingDate = g.BookingDate,
          Status = (int)g.Status,
          IsRated = g.IsRated,

      }).ToListAsync();

            var Booked = await db.Bookings.Where(d => d.IsDeleted == false && d.Status == BookingStatus.Booked && d.Ads.PartnerId == PartnerId && d.CheckedIn > DateTime.Now.Date && d.CheckOut > DateTime.Now.Date)
        .Select(g => new BookingDto
        {
            BookingId = g.BookingId,
            AdId = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.AdId).FirstOrDefault(),
            AdImage1 = db.Ads.Where(c => c.AdId == g.AdId).Select(g => ImagePath + g.AdImage1).FirstOrDefault(),
            NoOfBed = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfBed).FirstOrDefault(),
            NoOfPerson = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfPerson).FirstOrDefault(),
            Price = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.Price).FirstOrDefault(),
            RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault(),
            CheckedIn = g.CheckedIn,
            CheckOut = g.CheckOut,
            BookingNumber = db.Bookings.Where(b => b.IsDeleted == false && b.Status == BookingStatus.Booked && b.AdId == g.Ads.AdId).Select(q => q.BookingNumber).FirstOrDefault(),
            PaidStatus = (int)g.PaidStatus,
            BookingDate = g.BookingDate,
            Status = (int)g.Status,
            IsRated = g.IsRated
        }).ToListAsync();

            var Past = await db.Bookings.Where(d => d.IsDeleted == false && d.CheckedIn < DateTime.Now.Date && d.CheckOut < DateTime.Now.Date && d.Ads.PartnerId == PartnerId)
                .Select(g => new BookingDto
                {
                    BookingId = g.BookingId,
                    AdId = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.AdId).FirstOrDefault(),
                    AdImage1 = db.Ads.Where(c => c.AdId == g.AdId).Select(g => ImagePath + g.AdImage1).FirstOrDefault(),
                    NoOfBed = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfBed).FirstOrDefault(),
                    NoOfPerson = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.NoOfPerson).FirstOrDefault(),
                    Price = db.Ads.Where(c => c.AdId == g.AdId).Select(g => g.Price).FirstOrDefault(),
                    CheckedIn = g.CheckedIn,
                    CheckOut = g.CheckOut,
                    RoomType = db.Rooms.Where(k => k.RoomId == g.Ads.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                    BookingNumber = db.Bookings.Where(b => b.IsDeleted == false && b.Status == BookingStatus.Past && b.AdId == g.Ads.AdId).Select(q => q.BookingNumber).FirstOrDefault(),
                    PaidStatus = (int)g.PaidStatus,
                    BookingDate = g.BookingDate,
                    Status = (int)g.Status,
                    IsRated = g.IsRated,
                }).ToListAsync();
            bookingList.ActiveBookings = active;
            bookingList.BookedBookings = Booked;
            bookingList.PastBookings = Past;

            return bookingList;
        }
        public async Task<GetBookingOutPut> GetBookingById(int BookingId)
        {
            GetBookingOutPut getBooking = new GetBookingOutPut();

            getBooking = await db.Bookings.Where(g => g.BookingId == BookingId && g.IsDeleted == false).
            Select(g => new GetBookingOutPut
            {
                BookingId = g.BookingId,
                BookingNumber = g.BookingNumber,
                BookingDate = g.BookingDate,
                CustomerId = g.CustomerId,
                Price = g.Price,
                CommisionPrice = g.CommissionPrice,
                PaidStatus = (int)g.PaidStatus,
                PartnerId = (int)g.Ads.PartnerId,
                AdId = (int)g.Ads.AdId,
            }).FirstOrDefaultAsync();
            return getBooking;
        }

        public async Task UpdateBookingPaidStatus(int BookingId)
        {
            var booking = db.Bookings.Where(g => g.BookingId == BookingId).FirstOrDefault();
            booking.PaidStatus = CheckPaid.UnPaid;
            booking.ModifiedDate = DateTime.Now;
            db.Bookings.Update(booking);
            db.SaveChanges();
        }

        public async Task UpdateBookingStatus(int BookingId)
        {
            var booking = db.Bookings.Where(g => g.BookingId == BookingId).FirstOrDefault();
            booking.Status = BookingStatus.Rejected;
            booking.ModifiedDate = DateTime.Now;
            db.Bookings.Update(booking);
            db.SaveChanges();
        }
        public async Task UpdateBookingStatusToPaid(string ReferenceNumber)
        {
            var bookingid = db.Inquiries.Where(a => a.ReferenceNumber == ReferenceNumber).Select(b => b.BookingId).FirstOrDefault();
            var booking = db.Bookings.Where(g => g.BookingId == bookingid).FirstOrDefault();
            booking.Status = BookingStatus.Booked;
            booking.PaidStatus = CheckPaid.Paid;
            booking.ModifiedDate = DateTime.Now;
            db.Bookings.Update(booking);
            db.SaveChanges();
        }
        public async Task UpdateBookingStatusToPaidFromWallet(int BookingID)
        {
            var booking = db.Bookings.Where(g => g.BookingId == BookingID).FirstOrDefault();
            booking.Status = BookingStatus.Booked;
            booking.PaidStatus = CheckPaid.Paid;
            booking.ModifiedDate = DateTime.Now;
            db.Bookings.Update(booking);
            db.SaveChanges();
        }

        public async Task DeleteBooking(int BookingId)
        {
            var booking = db.Bookings.Where(g => g.BookingId == BookingId).FirstOrDefault();
            booking.IsDeleted = true;
            booking.ModifiedDate = DateTime.Now;
            db.Bookings.Update(booking);
            db.SaveChanges();
        }

        public async Task GiveRatingAndReview(int BookingID, decimal Rating, string Review, string ReviewName)
        {

            var dto = new BookingRatingAndReview
            {
                BookingId = BookingID,
                Rating = Rating,
                Review = Review,
                ReviewName = ReviewName
            };
            var booking = db.Bookings.Where(g => g.BookingId == BookingID).FirstOrDefault();
            booking.IsRated = true;
            db.BookingRatingAndReviews.Add(dto);
            db.Bookings.Update(booking);
            db.SaveChanges();
        }
    }
}
