using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Administration;
using LoftyRoomsModel.Bookings;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.ModelsVM;
using LoftyRoomsModel.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsDAL.Partners
{
    public class PartnerDAL
    {
        ApplicationDBContext db;
        private readonly IConfiguration configuration;
        public PartnerDAL(ApplicationDBContext _db, IConfiguration _configuration)
        {
            configuration = _configuration;
            db = _db;
        }
        public ResponseVM AddUpdatePartner(Partner model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.PartnerId == 0)
                {

                    var message = "";
                    if (db.Partners.Where(a => a.IsDeleted == false && a.Email == model.Email).Count() > 0 || db.Customers.Where(a => a.IsDeleted == false && a.Email == model.Email).Count() > 0)
                    {
                        message = "Email Already Exist!";
                        vm.StatusCode = -12;
                        vm.Message = message;
                    }
                    if (db.Partners.Where(a => a.IsDeleted == false && a.Phone == model.Phone).Count() > 0 || db.Customers.Where(a => a.IsDeleted == false && a.Mobile == model.Phone).Count() > 0)
                    {
                        message = "Mobile Already Exist!";
                        vm.StatusCode = -12;
                        vm.Message = message;
                    }
                    if (db.Partners.Where(a => a.IsDeleted == false && a.Email == model.Email && a.Phone == model.Phone).Count() > 0 || db.Customers.Where(a => a.IsDeleted == false && a.Email == model.Email && a.Mobile == model.Phone).Count() > 0)
                    {
                        message = "Email & Mobile Already Exist!";
                        vm.StatusCode = -12;
                        vm.Message = message;
                    }

                    if (message == "")
                    {
                        db.Partners.Add(model);
                        db.SaveChanges();
                        PartnerWallet(model.PartnerId);
                        vm.StatusCode = 11;
                        vm.Message = "Successfully Save Partner";
                    }
                    //else
                    //{
                    //    vm.StatusCode = -12; vm.Message = "Already Exist.";
                    //}
                }
                else
                {
                    var partner = db.Partners.Find(model.PartnerId);

                    partner.PartnerId = model.PartnerId;
                    partner.FirstName = model.FirstName;
                    partner.LastName = model.LastName;
                    partner.Email = model.Email;
                    partner.Password = model.Password;
                    partner.Address = model.Address;
                    partner.City = model.City;
                    partner.State = model.State;
                    partner.ZipCode = model.ZipCode;
                    partner.Phone = model.Phone;
                    partner.RoomId = model.RoomId;
                    partner.HotelName = model.HotelName;
                    partner.DateEntry = model.DateEntry;
                    partner.AllowLogin = model.AllowLogin;
                    partner.Latitude = model.Latitude;
                    partner.Longitude = model.Longitude;
                    partner.ModifiedBy = model.CreatedBy;
                    partner.ModifiedDate = DateTime.Now;
                    db.Entry(partner).State = EntityState.Modified;
                    db.SaveChanges();
                    vm.StatusCode = 11;
                    vm.Message = "Parter Update Successfully";
                }
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11;
                vm.Message = ex.Message;
                throw;
            }
            return vm;
        }

        public ResponseVM GetPartnerList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList = db.Partners.Where(p => p.IsDeleted == false).Select(g => new Partner

                {
                    PartnerId = g.PartnerId,
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    Email = g.Email,
                    Address = g.Address,
                    City = g.City,
                    State = g.State,
                    ZipCode = g.ZipCode,
                    Phone = g.Phone,
                    HotelName = g.HotelName
                }).OrderByDescending(x => x.PartnerId).ToList();

            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM GetRoomType()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList.roomTypes = db.Rooms.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }

        public ResponseVM GetPartnerById(Partner model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList = db.Partners.Find(model.PartnerId);
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM DeletePartner(Partner model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var partner = db.Partners.Find(model.PartnerId);
                partner.ModifiedBy = model.CreatedBy;
                partner.ModifiedDate = DateTime.Now;
                partner.IsDeleted = true;
                db.Entry(partner).State = EntityState.Modified;
                db.SaveChanges();
                vm.StatusCode = 11;
                vm.Message = "Successfully Delete";
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }


        public PartnerListDto Login(string Email, string Password, string AndroidFcmToken, string IosFcmToken)
        {
            try
            {
                var partner = db.Partners.Where(x => (x.Email == Email || x.Phone == Email) && x.Password == Password && x.IsDeleted == false)
                    .Select(g => new PartnerListDto
                    {
                        PartnerId = g.PartnerId,
                        FirstName = g.FirstName,
                        LastName = g.LastName,
                        Email = g.Email,
                        Address = g.Address,
                        Phone = g.Phone,
                    }).FirstOrDefault();
                if (AndroidFcmToken != "")
                {
                    var partnerAndroid = db.Partners.Where(x => (x.Email == Email || x.Phone == Email) && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
                    partnerAndroid.AndroidFcmToken = AndroidFcmToken;
                    db.Partners.Update(partnerAndroid);
                    db.SaveChanges();
                }
                if (IosFcmToken != "")
                {
                    var partnerIOS = db.Partners.Where(x => (x.Email == Email || x.Phone == Email) && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
                    partnerIOS.IosFcmToken = IosFcmToken;
                    db.Partners.Update(partnerIOS);
                    db.SaveChanges();
                }
                return partner;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<PartnerRoomListDto>> GetPartnerRoomList(int PartnerId)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            return await db.Ads.Where(d => d.IsDeleted == false && d.PartnerId == PartnerId)
                 .Select(g => new PartnerRoomListDto
                 {
                     AdId = g.AdId,
                     NoOfBed = g.NoOfBed,
                     NoOfPerson = g.NoOfPerson,
                     AdImage1 = ImagePath + g.AdImage1,
                     Price = g.BasePrice,
                     RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault()
                 }).ToListAsync();
        }

        public async Task<GetPartnerRoomDetail> GetPartnerRoomDetail(int AdId, int PartnerId)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            GetPartnerRoomDetail getRoomDetail = new GetPartnerRoomDetail();
            getRoomDetail.RoomDetail = await db.Ads.Where(g => g.AdId == AdId && g.PartnerId == PartnerId && g.IsDeleted == false).
                Select(g => new PartnerRoomDetailDto
                {
                    AdId = g.AdId,
                    NoOfBed = g.NoOfBed,
                    NoOfPerson = g.NoOfPerson,
                    Price = g.BasePrice,
                    RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                    Description = g.Description,
                    HotelName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.HotelName).FirstOrDefault(),
                    Location = g.Location,
                }).FirstOrDefaultAsync();

            var images = db.Ads.Where(g => g.AdId == AdId && g.PartnerId == PartnerId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage1)
     .Concat(db.Ads.Where(g => g.AdId == AdId && g.PartnerId == PartnerId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage2)).Concat(db.Ads.Where(g => g.AdId == AdId && g.PartnerId == PartnerId && g.IsDeleted == false)
     .Select(i => ImagePath + i.AdImage3)).Concat(db.Ads.Where(g => g.AdId == AdId && g.PartnerId == PartnerId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage4))
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
            getRoomDetail.FacilityList = facl;
            getRoomDetail.RoomImages = images;
            return getRoomDetail;
        }

        public async Task<PartnerPasswordDto> PartnerById(int PartnerId)
        {
            PartnerPasswordDto getPartner = new PartnerPasswordDto();

            getPartner = await db.Partners.Where(g => g.PartnerId == PartnerId && g.IsDeleted == false).
            Select(g => new PartnerPasswordDto
            {
                PartnerId = g.PartnerId,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Email = g.Email,
                Password = g.Password,
            }).FirstOrDefaultAsync();
            return getPartner;
        }
        public async Task<PartnerProfileListDto> GetPartnerProfileData(int PartnerId)
        {
            var ImagePath = configuration["LoftyImage:Image"];
            PartnerProfileListDto partnerProfile = new PartnerProfileListDto();

            partnerProfile.PartnerProfile = await db.Partners.Where(g => g.PartnerId == PartnerId && g.IsDeleted == false).Select(g => new PartnerProfile
            {
                PartnerId = g.PartnerId,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Email = g.Email,
                Address = g.Address,
                Phone = g.Phone,
                HotelName = g.HotelName,
                //HotelName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.HotelName).FirstOrDefault(),
            }).FirstOrDefaultAsync();

            partnerProfile.Review = db.BookingRatingAndReviews.Where(a => a.IsDeleted == false && a.Bookings.Ads.PartnerId == PartnerId)
                .Select(a => new PartnerReview
                {
                    Rating = a.Rating,
                    Review = a.Review,
                    ReviewName = a.ReviewName

                }).ToList();
            partnerProfile.PartnerAds = db.Ads.Where(a => a.IsDeleted == false && a.PartnerId == PartnerId).Select(g => new PartnerAdsDto
            {
                AdId = g.AdId,
                NoOfBed = g.NoOfBed,
                NoOfPerson = g.NoOfPerson,
                Price = g.BasePrice,
                RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                //HotelName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.HotelName).FirstOrDefault(),
                AdImage1 = db.Ads.Where(a => a.AdId == g.AdId && g.IsDeleted == false).Select(i => ImagePath + i.AdImage1).FirstOrDefault(),
            }).ToList();
            return partnerProfile;
        }

        public async Task<PartnerReview> GetPartnerRatingAnfReviews(int PartnerId)
        {
            PartnerReview partnerreview = new PartnerReview();
            partnerreview = (PartnerReview)db.BookingRatingAndReviews.Where(b => b.Bookings.Ads.PartnerId == PartnerId).Select(a => a.BookingId);
            return partnerreview;
        }

        public async Task UpdatePartnerPassword(string NewPassword, int PartnerId)
        {
            var partner = db.Partners.Where(g => g.PartnerId == PartnerId).FirstOrDefault();

            if (partner != null)
            {
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    partner.Password = NewPassword;
                }
                await db.SaveChangesAsync();
            }
        }


        public async Task<PartnerListDto> UpdatePartnerProfile(PartnerProfileEditDto input)
        {
            try
            {
                var dto = await db.Partners.Where(g => g.PartnerId == input.PartnerId).FirstOrDefaultAsync();
                dto.FirstName = input.FirstName;
                dto.LastName = input.LastName;
                dto.Address = input.Address;
                dto.ModifiedDate = DateTime.Now;

                db.Partners.Update(dto);
                db.SaveChanges();

                var response = new PartnerListDto
                {
                    PartnerId = dto.PartnerId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Address = dto.Address,
                    Phone = dto.Phone,
                };
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void PartnerWallet(int PartnerId)
        {
            var dto = new PartnerWalletAmount
            {
                PartnerId = PartnerId,
                WalletAnount = 0,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.PartnerWalletAmounts.Add(dto);
            db.SaveChanges();

        }
        public async Task<PartnerWalletHistoryTotalAmountDto> PartnerWalletHistory(int PartnerId)
        {
            PartnerWalletHistoryTotalAmountDto dto = new PartnerWalletHistoryTotalAmountDto();
            dto.PartnerWalletHistoryDto = db.PartnerWalletHistory
                 .Where(d => d.IsDeleted == false && d.PartnerId == PartnerId).Select(g => new PartnerWalletHistoryDto
                 {
                     PartnerName = db.Partners.Where(g => g.PartnerId == PartnerId).Select(a => a.FirstName + " " + a.LastName).FirstOrDefault(),
                     Type = g.Type,
                     Amount = g.Amount,
                 }).ToList();

            dto.TotalAmount = db.PartnerWalletHistory.Where(d => d.IsDeleted == false && d.PartnerId == PartnerId && d.Type == WalletType.Credit).Sum(item => item.Amount);

            return dto;
        }

        public async Task AddPartnerNotification(PartnerNotificationDto dto)
        {
            var notificaiton = new PartnerNotification
            {
                PartnerId = dto.PartnerId,
                Message = dto.Message,
                Title = dto.Title,
                CreatedDate = DateTime.Now
            };
            await db.AddAsync(notificaiton);
            await db.SaveChangesAsync();
        }


        public async Task<List<PartnerNotificationListDto>> GetAllPartnerNotification(int PartnerId)
        {
            return await db.PartnerNotifications.Where(k => k.IsDeleted == false && k.PartnerId == PartnerId)
                 .Select(g => new PartnerNotificationListDto
                 {
                     Message = g.Message,
                     Title = g.Title,
                     CreatedDate = g.CreatedDate,
                 }).ToListAsync();
        }
        public async Task DeletePartnerData(int PartnerId)
        {
            var partner = db.Partners.Where(g => g.PartnerId == PartnerId).FirstOrDefault();
            partner.IsDeleted = true;
            partner.ModifiedDate = DateTime.Now;
            db.Partners.Update(partner);
            db.SaveChanges();
        }


        public async Task<bool> AddPartnerWalletHistorWithCreditAmount(int PartnerId, decimal Price)
        {
            var dto = new PartnerWalletHistory
            {
                PartnerId = PartnerId,
                Amount = Price,
                Type = WalletType.Credit,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            };

            db.PartnerWalletHistory.Add(dto);
            db.SaveChanges();

            return true;

        }


        public async Task<bool> AddPartnerFirBaseNotification(string Title, string Body)
        {
            var dto = new PartnerFireBaseNotification
            {
                Title = Title,
                Body = Body,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };

            db.PartnerFireBaseNotifications.Add(dto);
            db.SaveChanges();
            return true;
        }
        public async Task<List<FireBaseNotificationListDto>> GetAllPartnerFireBaseNotification()
        {
            return await db.PartnerFireBaseNotifications.Where(d => d.IsDeleted == false)
                 .Select(g => new FireBaseNotificationListDto
                 {
                     NotificationId = g.NotificationId,
                     Title = g.Title,
                     Body = g.Body,
                     CreatedDate = g.CreatedDate,
                 }).ToListAsync();
        }


        public async Task<List<PartnerListWithTokens>> GetAllPartnersWithToken()
        {
            return await db.Partners.Where(d => d.IsDeleted == false)
                 .Select(g => new PartnerListWithTokens
                 {
                     CustomerName = g.FirstName + " " + g.LastName,
                     Email = g.Email,
                     Mobile = g.Phone,
                     AndroidFcmToken = g.AndroidFcmToken,
                     IosFcmToken = g.IosFcmToken
                 }).ToListAsync();
        }
        public bool SendNotificationToPartners(string Token, string Body, string Title)
        {
            try
            {
                var applicationID = "AAAAfY-8WWM:APA91bHKZQwDshkOykbf3dRYNd8IZuVDsh6huNyWHhU-NrKDILGk7ElrAWMwN9UgbLSMssFck9veZM5EbpVyEKFA5Jwh16esp-TSbvg7aK9s53hlRneQczwYHR55kJ4X-oo8XrXg-4QR";
                var senderId = "539282397539";
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
