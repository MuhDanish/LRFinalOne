using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Administration;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.ModelsVM;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace LoftyRoomsDAL.Administration
{
    public class AccountDAL
    {
        ApplicationDBContext db;
        public AccountDAL(ApplicationDBContext _db)
        {
            db = _db;
        }
        public ResponseVM Login(Acc_User login)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var user = db.Acc_Users.Where(x => x.UserName == login.UserName && x.Password == login.Password && x.IsDeleted == false).FirstOrDefault();
                if (user != null)
                {

                    var adminUser = (from c in db.acc_UserRoles
                                     join d in db.Acc_Roles on c.RoleId equals d.RoleId
                                     where c.UserId == user.UserId && d.RoleName == "Admin"
                                     select new
                                     {
                                         d.RoleName
                                     }
                                    ).FirstOrDefault();

                    if (adminUser != null)
                    {
                        user.Acc_Claims = db.Acc_Claims.ToList();
                    }
                    else
                    {
                        var query = (from a in db.acc_UserRoles
                                     join b in db.Acc_RoleClaims on a.RoleId equals b.RoleId
                                     join c in db.Acc_Claims on b.ClaimId equals c.ClaimId
                                     where a.UserId == user.UserId
                                     group c by new { c.ClaimId, c.ClaimName } into g
                                     select new
                                     {
                                         ClaimId = g.Key.ClaimId,
                                         ClaimName = g.Key.ClaimName,
                                         ModuleId = 0,
                                         Acc_UserUserId = 0
                                     }).ToList();

                        user.Acc_Claims = query.ToList()
                                                    .Select(item => new Acc_Claim
                                                    {
                                                        ClaimId = item.ClaimId,
                                                        ClaimName = item.ClaimName,
                                                    })
                                                    .ToList();

                    }

                    var claims = ""; var coma = "";
                    foreach (var item in user.Acc_Claims)
                    {
                        claims += coma + item.ClaimId.ToString();
                        coma = ",";
                    }

                    user.Claims = claims;
                    vm.StatusCode = 1;
                    vm.SuccessList = user;
                }
                else
                {
                    vm.StatusCode = 0;
                }
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11;
            }
            return vm;
        }


        public ResponseVM AddUpdateAcc_User(Acc_User model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.UserId == 0)
                {
                    if (db.Acc_Users.Where(x => x.UserName == model.UserName && x.LastName == model.LastName).Count() == 0)
                    {
                        db.Acc_Users.Add(model);
                        db.SaveChanges();
                        vm.StatusCode = 11;
                        vm.Message = "Successfully Save User";
                    }
                    else
                    {
                        vm.StatusCode = -12; vm.Message = "Already Exist.";
                    }
                }
                else
                {
                    var Acc_User = db.Acc_Users.Find(model.UserId);
                    Acc_User.ModifiedBy = model.CreatedBy;
                    Acc_User.ModifiedDate = DateTime.Now;
                    Acc_User.UserName = model.UserName;
                    Acc_User.LastName = model.LastName;
                    Acc_User.Email = model.Email;
                    Acc_User.Password = model.Password;
                    Acc_User.Cnic = model.Cnic;
                    Acc_User.Mobile = model.Mobile;
                    //Acc_User.RoleId = model.RoleId;
                    db.Entry(Acc_User).State = EntityState.Modified;
                    db.SaveChanges();
                    vm.StatusCode = 11;
                    vm.Message = "Successfully Update User";
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
        public ResponseVM DeleteAcc_User(Acc_User model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var Acc_User = db.Acc_Users.Find(model.UserId);
                Acc_User.ModifiedBy = model.CreatedBy;
                Acc_User.ModifiedDate = DateTime.Now;
                Acc_User.IsDeleted = true;
                db.Entry(Acc_User).State = EntityState.Modified;
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
        public ResponseVM GetUserDropDowns()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList.userTypes = db.Acc_Roles.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM GetUserList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList = db.Acc_Users.Where(p => p.IsDeleted == false).Select(g => new UserListDto
                {
                    UserId = g.UserId,
                    UserName = g.UserName,
                    LastName = g.LastName,
                    Email = g.Email,
                    Mobile = g.Mobile,
                    Cnic = g.Cnic,
                    //RoleId=g.RoleId,
                    //UserType = db.Acc_Roles.Where(b => b.RoleId == g.RoleId).Select(a => a.RoleName).FirstOrDefault(),
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM AddUpdateAcc_Role(Acc_Role model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                if (model.RoleId == 0)
                {
                    if (db.Acc_Roles.Where(x => x.RoleName == model.RoleName).Count() == 0)
                    {
                        db.Acc_Roles.Add(model);
                        db.SaveChanges();
                        vm.StatusCode = 11;
                        vm.Message = "Successfully Save Role";
                    }
                    else
                    {
                        vm.StatusCode = -12; vm.Message = "Already Exist.";
                    }
                }
                else
                {
                    var Acc_Role = db.Acc_Roles.Find(model.RoleId);
                    Acc_Role.RoleName = model.RoleName;
                    db.Entry(Acc_Role).State = EntityState.Modified;
                    db.SaveChanges();
                    vm.StatusCode = 11;
                    vm.Message = "Successfully Update Role";
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
        public ResponseVM GetAcc_RoleList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                //vm.SuccessList = db.Acc_Roles.ToList();
                vm.SuccessList = db.Acc_Roles.Where(a => a.RoleName != "Admin").ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }

        public ResponseVM AddUpdateRoleClaims(All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var claimslist = model.Claims.Split(',');
                Acc_RoleClaim roleclaims = new Acc_RoleClaim();
                foreach (var item in claimslist)
                {
                    roleclaims = new Acc_RoleClaim();
                    if (db.Acc_RoleClaims.Where(x => x.RoleId == model.RoleId && x.ClaimId.ToString() == item).Count() == 0)
                    {
                        roleclaims.RoleId = model.RoleId;
                        roleclaims.ClaimId = Convert.ToInt32(item);
                        db.Acc_RoleClaims.Add(roleclaims);
                        db.SaveChanges();
                    }
                    else
                    {
                        vm.StatusCode = -12; vm.Message = "Already Exist.";
                    }
                }


                /////////////////Delete Claims where not exists in current claims list///////
                var claimIds = model.Claims.Split(',').Select(int.Parse);
                var roleClaimsToDelete = db.Acc_RoleClaims
                    .Where(rc => rc.RoleId == model.RoleId && !claimIds.Contains(rc.ClaimId));
                db.Acc_RoleClaims.RemoveRange(roleClaimsToDelete);
                db.SaveChanges();

                vm.StatusCode = 11;
                vm.Message = "Successfully Assign Claims To Role";

            }
            catch (Exception ex)
            {
                vm.StatusCode = -11;
                vm.Message = ex.Message;
                throw;
            }
            return vm;
        }
        public ResponseVM GetClaimsList(All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {

                vm.SuccessList = from a in db.Acc_Claims
                                 join b in db.Acc_Modules on a.ModuleId equals b.ModuleId
                                 join c in db.Acc_RoleClaims
                                     on new { a.ClaimId, RoleId = model.RoleId }
                                     equals new { c.ClaimId, c.RoleId } into roleClaimsJoin
                                 from c in roleClaimsJoin.DefaultIfEmpty()
                                 select new
                                 {
                                     b.ModuleId,
                                     ClaimModuleId = a.ModuleId,
                                     b.ModuleName,
                                     a.ClaimName,
                                     a.ClaimId,
                                     IsSelected = (c.ClaimId != null && c.ClaimId > 0) ? 1 : 0
                                 };
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }

        public ResponseVM GetUserRoleList(All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm.SuccessList = from a in db.Acc_Roles
                                 join b in db.acc_UserRoles.Where(ur => ur.UserId == model.UserId)
                                     on a.RoleId equals b.RoleId into userRolesJoin
                                 from b in userRolesJoin.DefaultIfEmpty()
                                 select new
                                 {
                                     a.RoleId,
                                     a.RoleName,
                                     IsSelected = (b.UserRoleId != null && b.UserRoleId > 0) ? 1 : 0
                                 };
            }
            catch (Exception)
            {
                throw;
            }
            return vm;
        }
        public ResponseVM AddUpdateUserRole(All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                var rolesList = model.Roles.Split(',');
                Acc_UserRole user_roles = new Acc_UserRole();
                foreach (var item in rolesList)
                {
                    user_roles = new Acc_UserRole();
                    if (db.acc_UserRoles.Where(x => x.UserId == model.UserId && x.RoleId.ToString() == item).Count() == 0)
                    {
                        user_roles.UserId = model.UserId;
                        user_roles.RoleId = Convert.ToInt32(item);
                        db.acc_UserRoles.Add(user_roles);
                        db.SaveChanges();
                    }
                }

                var roleIds = model.Roles.Split(',').Select(int.Parse);
                var userRolesToDelete = db.acc_UserRoles
                    .Where(ur => ur.UserId == model.UserId && !roleIds.Contains(ur.RoleId));
                db.acc_UserRoles.RemoveRange(userRolesToDelete);
                db.SaveChanges();
                vm.StatusCode = 11;
                vm.Message = "Successfully Assign Roles To User";

            }
            catch (Exception ex)
            {
                vm.StatusCode = -11;
                vm.Message = ex.Message;
                throw;
            }
            return vm;
        }
    }
}
