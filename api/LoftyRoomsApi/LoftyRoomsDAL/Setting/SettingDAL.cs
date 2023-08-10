using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Ads;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Partners;
using LoftyRoomsModel.Setting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsDAL.Setting
{
    public class SettingDAL
    {
        ApplicationDBContext db;
        public SettingDAL(ApplicationDBContext _db)
        {
            db = _db;
        }
        #region Facility
        public ResponseVM AddUpdateFacility(Facility model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.FacilityId == 0)
                {
                    if (db.Facilities.Where(x => x.FacilityName == model.FacilityName).Count() == 0)
                    {
                        db.Facilities.Add(model);
                        db.SaveChanges();
                        vm.StatusCode = 11;
                        vm.Message = "Successfully Save Facility";
                    }
                    else
                    {
                        vm.StatusCode = -12; vm.Message = "Already Exist.";
                    }
                }
                else
                {
                    var facility = db.Facilities.Find(model.FacilityId);
                    facility.ModifiedBy = model.CreatedBy;
                    facility.ModifiedDate = DateTime.Now;
                    facility.FacilityName = model.FacilityName;
                    facility.Count = model.Count;
                    if (model.Image != "")
                    {
                        facility.Image = model.Image;
                    }
                    db.Entry(facility).State = EntityState.Modified;
                    db.SaveChanges();
                    vm.StatusCode = 11;
                    vm.Message = "Successfully Update Facility";
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
        public ResponseVM GetFacilityList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList = db.Facilities.Where(x => x.IsDeleted == false).OrderByDescending(x => x.FacilityId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM DeleteFacility(Facility model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var facility = db.Facilities.Find(model.FacilityId);
                facility.ModifiedBy = model.CreatedBy;
                facility.ModifiedDate = DateTime.Now;
                facility.IsDeleted = true;
                db.Entry(facility).State = EntityState.Modified;
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

        public ResponseVM getFacilityById(Facility model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {


                vm.SuccessList = (from c in db.Facilities
                                  where c.IsDeleted == false && c.FacilityId == model.FacilityId
                                  select new
                                  {
                                      c.FacilityId,
                                      c.FacilityName,
                                      c.Image,
                                      c.Count,

                                  }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        #endregion

        #region RoomType

        public ResponseVM AddUpdateRoomType(Room model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.RoomId == 0)
                {
                    if (db.Rooms.Where(x => x.RoomType == model.RoomType).Count() == 0)
                    {
                        db.Rooms.Add(model);
                        db.SaveChanges();
                        vm.StatusCode = 11;
                        vm.Message = "Successfully Save Room Type";
                    }
                    else
                    {
                        vm.StatusCode = -12; vm.Message = "Already Exist.";
                    }
                }
                else
                {
                    var room = db.Rooms.Find(model.RoomId);
                    room.ModifiedBy = model.CreatedBy;
                    room.ModifiedDate = DateTime.Now;
                    room.RoomType = model.RoomType;
                    db.Entry(room).State = EntityState.Modified;
                    db.SaveChanges();
                    vm.StatusCode = 11;
                    vm.Message = "Successfully Update Room Type";
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
        public ResponseVM GetRoomTypeList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList.roomTypes = db.Rooms.Where(x => x.IsDeleted == false).OrderByDescending(x => x.RoomId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM DeleteRoomType(Room model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var room = db.Rooms.Find(model.RoomId);
                room.ModifiedBy = model.CreatedBy;
                room.ModifiedDate = DateTime.Now;
                room.IsDeleted = true;
                db.Entry(room).State = EntityState.Modified;
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
        #endregion

        #region Persons

        public ResponseVM AddUpdatePerson(Person model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.PersonId == 0)
                {
                    if (db.Persons.Where(x => x.NoOfPerson == model.NoOfPerson).Count() == 0)
                    {
                        db.Persons.Add(model);
                        db.SaveChanges();
                        vm.StatusCode = 11;
                        vm.Message = "Successfully Save Persons";
                    }
                    else
                    {
                        vm.StatusCode = -12; vm.Message = "Already Exist.";
                    }
                }
                else
                {
                    var person = db.Persons.Find(model.PersonId);
                    person.ModifiedBy = model.CreatedBy;
                    person.ModifiedDate = DateTime.Now;
                    person.NoOfPerson = model.NoOfPerson;
                    db.Entry(person).State = EntityState.Modified;
                    db.SaveChanges();
                    vm.StatusCode = 11;
                    vm.Message = "Successfully Update Persons";
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
        public ResponseVM GetPersonsList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList.persons = db.Persons.Where(x => x.IsDeleted == false).OrderByDescending(x => x.PersonId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM DeletePerson(Person model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var per = db.Persons.Find(model.PersonId);
                per.ModifiedBy = model.CreatedBy;
                per.ModifiedDate = DateTime.Now;
                per.IsDeleted = true;
                db.Entry(per).State = EntityState.Modified;
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
        #endregion

        #region Package
        public async Task<ResponseVM> AddUpdatePackage(Package model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.PackageId == 0)
                {
                    if (db.Packages.Where(x => x.PromotionalText == model.PromotionalText).Count() == 0)
                    {

                        var dto = new Package
                        {
                            PromotionalText = model.PromotionalText,
                            DiscountPercentage = model.DiscountPercentage,
                            PromotionalImage = model.PromotionalImage,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            CreatedBy = model.CreatedBy,
                        };
                        db.Packages.Add(dto);
                        db.SaveChanges();
                        vm.StatusCode = 11;
                        vm.Message = "Successfully Save Package";
                    }
                    else
                    {
                        vm.StatusCode = -12; vm.Message = "Already Exist.";
                    }
                }
                else
                {
                    var package = db.Packages.Find(model.PackageId);
                    package.ModifiedBy = model.CreatedBy;
                    package.ModifiedDate = DateTime.Now;
                    package.PromotionalText = model.PromotionalText;
                    package.DiscountPercentage = model.DiscountPercentage;
                    if (model.PromotionalImage != "")
                    {
                        package.PromotionalImage = model.PromotionalImage;
                    }
                    db.Entry(package).State = EntityState.Modified;
                    db.SaveChanges();
                    vm.StatusCode = 11;
                    vm.Message = "Successfully Update Package";
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
        public ResponseVM getPackageById(Package model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList = (from c in db.Packages
                                  where c.IsDeleted == false && c.PackageId == model.PackageId
                                  select new
                                  {
                                      c.PackageId,
                                      c.PromotionalText,
                                      c.DiscountPercentage,
                                      c.PromotionalImage
                                  }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM DeletePackage(Package model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var pkg = db.Packages.Find(model.PackageId);
                pkg.ModifiedBy = model.CreatedBy;
                pkg.ModifiedDate = DateTime.Now;
                pkg.IsDeleted = true;
                db.Entry(pkg).State = EntityState.Modified;
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
        public ResponseVM GetPackageList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList.package = db.Packages.Where(x => x.IsDeleted == false).OrderByDescending(x => x.PackageId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }

        #endregion
    }
}
