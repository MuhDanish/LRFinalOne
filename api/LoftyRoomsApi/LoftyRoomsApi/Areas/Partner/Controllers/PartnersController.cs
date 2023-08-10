using LoftyRoomsApi.Common;
using LoftyRoomsDAL.Administration;
using LoftyRoomsDAL.Booking;
using LoftyRoomsDAL.Customers;
using LoftyRoomsDAL.DBContext;
using LoftyRoomsDAL.Partners;
using LoftyRoomsDAL.Setting;
using LoftyRoomsModel.Administration;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.ModelsVM;
using LoftyRoomsModel.Partners;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LoftyRoomsApi.Areas.Partner.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : Controller
    {
        private IConfiguration _config;
        private ApplicationDBContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public PartnersController(IConfiguration config, ApplicationDBContext _db, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            db = _db;
            webHostEnvironment = hostEnvironment;
        }
        [IsAuthorize("11")]
        [HttpPost]
        [Route("AddUpdatePartner")]
        public ResponseVM AddUpdatePartner()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                LoftyRoomsModel.Partners.Partner model = new LoftyRoomsModel.Partners.Partner();
                var data = HttpContext.Request.Form["Model"];
                model = JsonConvert.DeserializeObject<LoftyRoomsModel.Partners.Partner>(data);
                vm = new PartnerDAL(db, _config).AddUpdatePartner(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = 0; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("12")]
        [HttpGet]
        [Route("GetPartnerList")]
        public ResponseVM GetPartnerList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new PartnerDAL(db, _config).GetPartnerList();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        [IsAuthorize("12")]
        [HttpPost]
        [Route("DeletePartner")]
        public ResponseVM DeletePartner([FromBody] LoftyRoomsModel.Partners.Partner model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new PartnerDAL(db, _config).DeletePartner(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }


        [IsAuthorize("11")]
        [HttpPost]
        [Route("GetPartnerById")]
        public ResponseVM GetPartnerById([FromBody] LoftyRoomsModel.Partners.Partner model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new PartnerDAL(db, _config).GetPartnerById(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [HttpGet]
        [Route("GetRoomTypeList")]
        public ResponseVM GetRoomTypeList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).GetRoomTypeList();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        #region Partner App Side

        private string GenerateJSONWebTokenFormPartner(PartnerListDto partnerInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
                {
                   new Claim("Id", partnerInfo.PartnerId.ToString()),
                    new Claim("PartnerName", partnerInfo.FirstName +" "+partnerInfo.LastName)
                };
            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              authClaims,
              expires: DateTime.Now.AddYears(10),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(PartnerDto login)
        {
            try
            {
                var response = new PartnerDAL(db, _config).Login(login.Email, login.Password, login.AndroidFcmToken, login.IosFcmToken);
                if (response != null)
                {
                    var token = GenerateJSONWebTokenFormPartner(response);
                    response.Token = token;
                    return Json(new { Result = "success", Message = "Authenticate Partner", data = response });
                }
                else
                {

                    return Json(new { Result = "error", Message = "Email or Password is InCorrect!", data = response });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetPartnerAllRoomsList")]
        public async Task<IActionResult> GetPartnerAllRoomsList()
        {
            try
            {
                ClaimsPrincipal partner = HttpContext.User;
                string Id = partner.FindFirstValue("Id");
                var PartnerId = int.Parse(Id);
                var result = await new PartnerDAL(db, _config).GetPartnerRoomList(PartnerId);
                if (result.Count > 0)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }
                else
                {
                    return Json(new { Result = "success", Message = "Data Not Found!", data = result });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetRoomDetail")]
        public async Task<IActionResult> GetRoomDetail(int AdId)
        {
            try
            {
                ClaimsPrincipal partner = HttpContext.User;
                string Id = partner.FindFirstValue("Id");
                var PartnerId = int.Parse(Id);
                var result = await new PartnerDAL(db, _config).GetPartnerRoomDetail(AdId, PartnerId);
                if (result.RoomDetail != null)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!", data = result });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetPartnerProfile")]
        public async Task<IActionResult> GetPartnerProfile(int PartnerId)
        {
            try
            {
                var result = await new PartnerDAL(db, _config).GetPartnerProfileData(PartnerId);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!", data = result });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("UpdatePartnerProfile")]
        public async Task<IActionResult> UpdatePartnerProfile(PartnerProfileEditDto input)
        {
            try
            {

                var result = await new PartnerDAL(db, _config).UpdatePartnerProfile(input);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Successfully Update Partner Profile!", data = result });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Some thing is went wrong!", data = result });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("PartnerChangePassword")]
        public async Task<IActionResult> PartnerChangePassword(PartnerChangePasswordDto inputDto)
        {
            try
            {
                var result = await new PartnerDAL(db, _config).PartnerById(inputDto.PartnerId);
                if (result.Password == inputDto.OldPassword)
                {
                    await new PartnerDAL(db, _config).UpdatePartnerPassword(inputDto.NewPassword, inputDto.PartnerId);
                    return Json(new { Result = "success", Message = "Successfully Password Change!", });
                }
                else
                {
                    return Json(new { Result = "error", Message = "InCorrect Old Password!", });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("GetPartnerWalletTotalAmouont")]
        public async Task<IActionResult> GetPartnerWalletTotalAmouont(int PartnerId)
        {
            try
            {
                var result = await new PartnerDAL(db, _config).PartnerWalletHistory(PartnerId);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }

                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetPartnerWalletHistory")]
        public async Task<IActionResult> GetPartnerWalletHistory(int PartnerId)
        {
            try
            {
                var result = await new PartnerDAL(db, _config).PartnerWalletHistory(PartnerId);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }

                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllPartnerBooking")]
        public async Task<IActionResult> GetAllPartnerBooking()
        {
            try
            {
                ClaimsPrincipal partner = HttpContext.User;
                string Id = partner.FindFirstValue("Id");
                int PartnerId = int.Parse(Id);
                var result = await new BookingDAL(db, _config).GetAllPartnerBookings(PartnerId);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!", data = result });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllPartnerNotification")]
        public async Task<IActionResult> GetAllPartnerNotification()
        {

            try
            {
                ClaimsPrincipal partner = HttpContext.User;
                string Id = partner.FindFirstValue("Id");
                int PartnerId = int.Parse(Id);
                var result = await new PartnerDAL(db, _config).GetAllPartnerNotification(PartnerId);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!", data = result });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("DeletePartnerData")]
        public async Task<IActionResult> DeletePartnerData(int PartnerId)
        {
            try
            {
                await new PartnerDAL(db, _config).DeletePartnerData(PartnerId);
                return Json(new { Result = "success", Message = "Partner Delete SuccessFully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllPartnerFireBaseNotification")]
        public async Task<IActionResult> GetAllPartnerFireBaseNotification()
        {
            try
            {
                var result = await new PartnerDAL(db, _config).GetAllPartnerFireBaseNotification();
                if (result.Count > 0)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!", data = result });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("SendNotificationToPartner")]
        public async Task<IActionResult> SendNotificationToPartner(FirBaseNotificationDto input)
        {
            try
            {
                bool result = false;
                var list = await new PartnerDAL(db, _config).GetAllPartnersWithToken();
                foreach (var item in list)
                {
                    if (item.AndroidFcmToken != null)
                    {
                        result = new PartnerDAL(db, _config).SendNotificationToPartners(item.AndroidFcmToken, input.Body, input.Title);
                    }
                    if (item.IosFcmToken != null)
                    {
                        result = new PartnerDAL(db, _config).SendNotificationToPartners(item.IosFcmToken, input.Body, input.Title);
                    }

                }
                if (result == true)
                {
                    await new PartnerDAL(db, _config).AddPartnerFirBaseNotification(input.Title, input.Body);
                    return Json(new { Result = "success", Message = "Notification Send To Partner Successfully!" });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Notification Not Send to Partner!" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        #endregion

    }
}
