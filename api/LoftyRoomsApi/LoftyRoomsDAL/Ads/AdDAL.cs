using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Ads;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.Partners;
using LoftyRoomsModel.Setting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsDAL.Ads
{
    public class AdDAL
    {
        ApplicationDBContext db;
        public AdDAL(ApplicationDBContext _db)
        {
            db = _db;
        }
        public ResponseVM GetAdsDropDowns()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList.persons = db.Persons.Where(x => x.IsDeleted == false).ToList();
                vm.SuccessList.partners = db.Partners.Where(x => x.IsDeleted == false).ToList();
                vm.SuccessList.roomsType = db.Rooms.Where(x => x.IsDeleted == false).ToList();
                vm.SuccessList.adsTypes = db.AdTypes.Where(x => x.IsDeleted == false).ToList();
                vm.SuccessList.packages = db.Packages.Where(x => x.IsDeleted == false).ToList();
                vm.SuccessList.facilities = db.Facilities.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vm;
        }

        public ResponseVM AddUpdateAds(AdDto model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.AdId == 0)
                {


                    var dto = new Ad
                    {
                        PersonId = model.PersonId,
                        NoOfBed = model.NoOfBed,
                        NoOfPerson = model.NoOfPerson,
                        PartnerId = model.PartnerId,
                        Price = model.Price,
                        BasePrice = model.BasePrice,
                        RoomId = model.RoomId,
                        AdTypeId = model.AdTypeId,
                        AdImage1 = model.AdImage1,
                        AdImage2 = model.AdImage2,
                        AdImage3 = model.AdImage3,
                        AdImage4 = model.AdImage4,
                        PackageId = model.PackageId,
                        PackageStartDate = model.PackageStartDate,
                        PackageEndDate = model.PackageEndDate,
                        Description = model.Description,
                        Location = model.Location,
                        RoomNo = model.RoomNo,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        CreatedBy = model.CreatedBy,
                    };
                    var savead = db.Ads.Add(dto);
                    db.SaveChanges();


                    if (model.Facilities.Count() > 0)
                    {
                        foreach (var facilityId in model.Facilities)
                        {
                            SaveFacilty(savead.Entity.AdId, facilityId);
                        }
                    }

                    vm.StatusCode = 11;
                    vm.Message = "Successfully Save Ad";

                }
                else
                {
                    var add = db.Ads.Find(model.AdId);
                    add.ModifiedBy = model.CreatedBy;
                    add.ModifiedDate = DateTime.Now;
                    add.NoOfBed = model.NoOfBed;
                    add.NoOfPerson = model.NoOfPerson;
                    add.PartnerId = model.PartnerId;
                    add.Price = model.Price;
                    add.BasePrice = model.BasePrice;
                    add.RoomId = model.RoomId;
                    add.AdTypeId = model.AdTypeId;
                    add.PackageId = model.PackageId;
                    add.PackageStartDate = model.PackageStartDate;
                    add.PackageEndDate = model.PackageEndDate;
                    add.Description = model.Description;
                    add.Location = model.Location;
                    add.RoomNo = model.RoomNo;
                    add.PersonId = model.PersonId;
                    if (model.AdImage1 != "")
                    {
                        add.AdImage1 = model.AdImage1;
                    }
                    if (model.AdImage2 != "")
                    {
                        add.AdImage2 = model.AdImage2;
                    }
                    if (model.AdImage3 != "")
                    {
                        add.AdImage3 = model.AdImage3;
                    }
                    if (model.AdImage4 != "")
                    {
                        add.AdImage4 = model.AdImage4;
                    }
                    db.Entry(add).State = EntityState.Modified;
                    db.SaveChanges();

                    var removeAdFacility = db.AdFacilities.Where(v => v.AdId == model.AdId).ToList();

                    db.AdFacilities.RemoveRange(removeAdFacility);
                    db.SaveChanges();

                    if (model.Facilities.Count() > 0)
                    {
                        foreach (var facilityId in model.Facilities)
                        {
                            var adfac = new AdFacility
                            {
                                FacilityId = facilityId,
                                AdId = model.AdId
                            };
                            db.AdFacilities.Add(adfac);
                        }
                        db.SaveChanges();
                    }
                    vm.StatusCode = 11;
                    vm.Message = "Successfully Update Ad";
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
        private void SaveFacilty(int AdId, int FacilityId)
        {
            var fac = new AdFacility
            {
                AdId = AdId,
                FacilityId = FacilityId
            };
            db.Add(fac);
            db.SaveChanges();
        }

        public ResponseVM getAdById(Ad model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {


                vm.SuccessList.OriginalAd = (from c in db.Ads
                                             where c.IsDeleted == false && c.AdId == model.AdId
                                             select new
                                             {
                                                 c.AdId,
                                                 c.PersonId,
                                                 c.NoOfBed,
                                                 c.NoOfPerson,
                                                 c.PartnerId,
                                                 c.Price,
                                                 c.BasePrice,
                                                 c.RoomId,
                                                 c.AdTypeId,
                                                 c.AdImage1,
                                                 c.AdImage2,
                                                 c.AdImage3,
                                                 c.AdImage4,
                                                 c.PackageId,
                                                 c.PackageStartDate,
                                                 c.PackageEndDate,
                                                 c.Description,
                                                 c.Location,
                                                 c.RoomNo,
                                             }).ToList();
                vm.SuccessList.fac = db.AdFacilities.Where(v => v.AdId == model.AdId).Select(g => new
                {
                    g.FacilityId
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM DeleteAds(Ad model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var add = db.Ads.Find(model.AdId);
                add.ModifiedBy = model.CreatedBy;
                add.ModifiedDate = DateTime.Now;
                add.IsDeleted = true;
                db.Entry(add).State = EntityState.Modified;
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
        public ResponseVM GetAdListList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList.adds = db.Ads.Where(x => x.IsDeleted == false).OrderByDescending(x => x.AdId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }

        public async Task<List<AdListDto>> GetAllAdsList()
        {
            return await db.Ads.Where(d => d.IsDeleted == false)
                 .Select(g => new AdListDto
                 {
                     AdId = g.AdId,
                     NoOfBed = g.NoOfBed,
                     NoOfPerson = g.NoOfPerson,
                     Price = g.Price,
                     Description = g.Description,
                     RoomType = db.Rooms.Where(k => k.RoomId == g.RoomId).Select(v => v.RoomType).FirstOrDefault(),
                     PartnerName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(a => a.FirstName + " " + a.LastName).FirstOrDefault(),
                     HotelName = db.Partners.Where(a => a.IsDeleted == false && a.PartnerId == g.PartnerId).Select(k => k.HotelName).FirstOrDefault(),
                 }).ToListAsync();
        }

    }
}
