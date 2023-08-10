using LoftyRoomsApi.Common;
using LoftyRoomsDAL.Administration;
using LoftyRoomsDAL.DBContext;
using LoftyRoomsModel.Administration;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.ModelsVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace LoftyRoomsApi.Areas.Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDBContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AccountController(IConfiguration config, ApplicationDBContext _db, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            db = _db;
            webHostEnvironment = hostEnvironment;
        }

        //********************************Login**********************************************
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public ResponseVM Login([FromBody] Acc_User login)
        {
            ResponseVM vm = new ResponseVM();
            Acc_User user = new Acc_User();
            try
            {
                IActionResult response = Unauthorized();
                vm = new AccountDAL(db).Login(login);
                if (vm.StatusCode == 1)
                {
                    user = vm.SuccessList;
                    var token = GenerateJSONWebToken(user);
                    user.Token = token;
                    vm.StatusCode = 1;
                    user.Acc_Claims = null;
                    vm.SuccessList = user;
                }
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        private string GenerateJSONWebToken(Acc_User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
                {
                   new Claim("Id", userInfo.UserId.ToString()),
                    new Claim("UserName", userInfo.UserName)
                };
            foreach (var userclaims in userInfo.Acc_Claims)
            {
                authClaims.Add(new Claim(userclaims.ClaimId.ToString(), userclaims.ClaimId.ToString()));
            }

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              authClaims,
              expires: DateTime.Now.AddYears(10),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //********************************User**********************************************
        [IsAuthorize("1")]
        [HttpPost]
        [Route("AddUpdateUser")]
        public ResponseVM AddUpdateUser([FromBody] Acc_User login)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).AddUpdateAcc_User(login);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("3")]
        [HttpPost]
        [Route("DeleteUser")]
        public ResponseVM DeleteUser([FromBody] Acc_User login)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).DeleteAcc_User(login);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        //********************************Role**********************************************
        [IsAuthorize("4")]
        [HttpPost]
        [Route("AddUpdateRole")]
        public ResponseVM AddUpdateRole([FromBody] Acc_Role model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).AddUpdateAcc_Role(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [HttpGet]
        [Route("GetUserDropDowns")]
        public ResponseVM GetUserDropDowns()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).GetUserDropDowns();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        [HttpGet]
        [Route("GetUserList")]
        public ResponseVM GetUserList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).GetUserList();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("5")]
        [HttpGet]
        [Route("GetRoleList")]
        public ResponseVM GetRoleList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).GetAcc_RoleList();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        //********************************Role Claims**********************************************
        [IsAuthorize("7")]
        [HttpPost]
        [Route("AddUpdateRoleClaims")]
        public ResponseVM AddUpdateRoleClaims([FromBody] All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).AddUpdateRoleClaims(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("8")]
        [HttpPost]
        [Route("GetClaimsList")]
        public ResponseVM GetClaimsList(All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).GetClaimsList(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        //********************************Assign Roles**********************************************
        [IsAuthorize("9")]
        [HttpPost]
        [Route("AddUpdateUserRole")]
        public ResponseVM AddUpdateUserRole(All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).AddUpdateUserRole(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("10")]
        [HttpPost]
        [Route("GetUserRoleList")]
        public ResponseVM GetUserRoleList(All_VM model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AccountDAL(db).GetUserRoleList(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
    }
}
