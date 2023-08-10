using LoftyRoomsDAL.Booking;
using LoftyRoomsDAL.Customers;
using LoftyRoomsDAL.DBContext;
using LoftyRoomsDAL.Partners;
using LoftyRoomsDAL.Setting;
using LoftyRoomsModel.Bookings;
using LoftyRoomsModel.Customers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LoftyRoomsApi.Areas.Customer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private IConfiguration _config;
        private ApplicationDBContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private HttpClient _httpClient;
        public CustomerController(IConfiguration config, ApplicationDBContext _db, IWebHostEnvironment hostEnvironment, HttpClient httpClient)
        {
            _config = config;
            db = _db;
            webHostEnvironment = hostEnvironment;
            _httpClient = httpClient;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(CustomerDto login)
        {
            try
            {
                var response = new CustomerDAL(db, _config).Login(login.Email, login.Password, login.AndroidFcmToken, login.IosFcmToken);
                if (response != null)
                {
                    var token = GenerateJSONWebTokenFormCustomer(response);
                    response.Token = token;
                    return Json(new { Result = "success", Message = "Authenticate Customer", data = response });
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

        private string GenerateJSONWebTokenFormCustomer(CustomerListDto customerInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
                {
                   new Claim("Id", customerInfo.CustomerId.ToString()),
                    new Claim("CustomerName", customerInfo.CustomerName)
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
        [Route("AddUpdateCustomer")]
        public async Task<IActionResult> AddUpdateCustomer([FromForm] CustomerEditDto inputDto)
        {
            try
            {
                var Image = HttpContext.Request.Form.Files["ImagePath"];
                var ImageFilePath = new LoftyRoomsApi.Common.CommonFunctions(webHostEnvironment).UploadProfileImage(Image);
                if (ImageFilePath != "")
                {
                    inputDto.ImagePath = ImageFilePath;
                }
                else
                {
                    inputDto.ImagePath = "";
                }
                var customer = await new CustomerDAL(db, _config).AddUpdateCustomer(inputDto);

                if (customer.CustomerId >= 1)
                {
                    var token = GenerateJSONWebTokenFormCustomer(customer);
                    customer.Token = token;
                    return Json(new { Result = "success", Message = "Successfully Save Customer!", data = customer });
                }
                else
                {
                    return BadRequest(new { Result = "error", Message = "Customer Already Exist With this Email or Mobile Number!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllRoomsCityWise")]
        public async Task<IActionResult> GetAllRoomsCityWise(string CityName)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetAllRoomCityWise(CityName);
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
        [HttpGet]
        [Route("GetAllRoomsList")]
        public async Task<IActionResult> GetAllRoomsList()
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetAllRoomList();
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
        [HttpGet]
        [Route("GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetAllCityList();
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
        [HttpGet]
        [Route("GetRoomDeatil")]
        public async Task<IActionResult> GetRoomDeatil(int AdId)
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId;
                //int CustomerId = int.Parse(Id);
                if (Id == null)
                {
                    CustomerId = 0;
                }
                else
                {
                    CustomerId = int.Parse(Id);
                }
                var result = await new CustomerDAL(db, _config).GetRoomDetail(AdId, CustomerId);
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
        [Route("GetAllPackageRoomsList")]
        public async Task<IActionResult> GetAllPackageRoomsList()
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetAllPackageRoomList();
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


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto inputDto)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetCustomerById(inputDto.CustomerId);
                if (result.Password == inputDto.OldPassword)
                {
                    await new CustomerDAL(db, _config).UpdatePassword(inputDto.NewPassword, inputDto.CustomerId);
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
        [Route("IsFavouriteRoom")]
        public async Task<IActionResult> IsFavouriteRoom(int AdId, bool IsFavourite)
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId = int.Parse(Id);
                var result = await new CustomerDAL(db, _config).GetRoomDetail(AdId, CustomerId);
                if (result.RoomDetail != null)
                {
                    await new CustomerDAL(db, _config).AddToFavourite(AdId, IsFavourite, CustomerId);
                    if (IsFavourite == true)
                    {
                        return Json(new { Result = "success", Message = "Room Add To Favourite!" });
                    }
                    else
                    {
                        return Json(new { Result = "success", Message = "Room Add To UnFavourite!" });
                    }

                }
                else
                {
                    return Json(new { Result = "error", Message = "Data Not Found!", });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetAllFavouriteRoomList")]
        public async Task<IActionResult> GetAllFavouriteRoomList()
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId = int.Parse(Id);
                var result = await new CustomerDAL(db, _config).GetAllFavouriteRoom(CustomerId);
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
        [HttpGet]
        [Route("GetAllNearByRooms")]
        public async Task<IActionResult> GetAllNearByRooms(double latitude, double longitude, double radius)
        {
            try
            {

                var result = await new CustomerDAL(db, _config).GetAllNearByRooms(latitude, longitude, radius);
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
        [Route("GetAllFilterRoomsList")]
        public async Task<IActionResult> GetAllFilterRoomsList(FilterRoomListDto input)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetAllFilterRoomList(input);
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

        #region CustomerWallet

        [HttpGet]
        [Route("GetWalletTotalAmount")]
        public async Task<IActionResult> GetWalletTotalAmount(int CustomerId)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetVoucherTotalAmount(CustomerId);
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
        [HttpPost]
        [Route("GetVoucherCode")]
        public async Task<IActionResult> GetVoucherCode(VouchePriceDto input)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).AddVoucherAmount(input);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Amount Added Successfully!", data = result });
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

        [HttpPost]
        [Route("AddWalletAmount")]
        public async Task<IActionResult> AddWalletAmount(VoucherAmountDto input)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).AddWalletAmount(input);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Amount Added Successfully!", data = result });
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

        [HttpGet]
        [Route("GetCustomerWalletHistory")]
        public async Task<IActionResult> GetCustomerWalletHistory(int CustomerId)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).CustomerWalletHistory(CustomerId);
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

        [HttpPost]
        [Route("AddCustomerHelpSupport")]
        public async Task<IActionResult> AddCustomerHelpSupport(string Message)
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId = int.Parse(Id);
                var result = await new CustomerDAL(db, _config).CustomerHelpSupport(CustomerId, Message);
                if (result == true)
                {
                    return Json(new { Result = "success", Message = "Your Request has been Submitted!" });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Some Thing Went Wrong!" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("CustomerRatingAndReviews")]
        public async Task<IActionResult> CustomerRatingAndReviews(CustomerRatingDto input)
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId = int.Parse(Id);
                var result = await new CustomerDAL(db, _config).AddCustomerRatingAndReview(CustomerId, input);
                if (result == true)
                {
                    return Json(new { Result = "success", Message = "Your Response has been Submitted!" });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Some Thing Went Wrong!" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("GetCustomerBooking")]
        public async Task<IActionResult> GetCustomerBooking(int BookingId)
        {
            try
            {
                var result = await new BookingDAL(db, _config).GetBookingById(BookingId);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Data Found!", data = result });

                }
                else
                {
                    //var bookingiquiry = new BookingDAL(db, _config).AddInquiry(result.BookingId, result.BookingNumber, result.BookingDate, result.Price);
                    return Json(new { Result = "error", Message = "Data Not Found!", data = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("PayThroughPayit")]
        public async Task<IActionResult> PayThroughPayit(int BookingId, bool IsThroughPayIT)
        {
            try
            {
                var result = await new BookingDAL(db, _config).GetBookingById(BookingId);

                if (result != null && IsThroughPayIT == true)
                {
                    var bookingiquiry = new BookingDAL(db, _config).AddInquiry(result.BookingId, result.BookingNumber, result.BookingDate, result.Price);
                    return Json(new { Result = "success", Message = "Data Found!", data = bookingiquiry.Result });
                }
                else if (result != null && IsThroughPayIT == false) // payment is from wallet
                {
                    var walletpayment = await new BookingDAL(db, _config).PaymentThroughWallet(result.CustomerId, result.Price, result.CommisionPrice, result.AdId);
                    if (walletpayment == true)
                    {
                        await new BookingDAL(db, _config).UpdateBookingStatusToPaidFromWallet(result.BookingId);

                        //write a method here to maintain the histoy of the wallet debiit
                        await new CustomerDAL(db, _config).AddCustomerWalletHistorWithDebit(result.CustomerId, result.Price);

                        //write a method here to maintain the histoy of the Partner wallet  Credit
                        await new PartnerDAL(db, _config).AddPartnerWalletHistorWithCreditAmount(result.PartnerId, result.Price);

                        var res = new GetInquiryOutPut
                        {
                            BookingId = 0,
                            ReferenceNumber = "",
                            BankAccountCode = "",
                            X_API_KEY = "",
                        };
                        return Json(new { Result = "success", Message = "Your booking payment have been made from Wallet!", data = res });
                    }
                    else
                    {
                        return Json(new { Result = "error", Message = "Insufficient Balance!" });
                    }
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
        [HttpPost]
        [Route("GetInquiryData")]
        public async Task<IActionResult> GetInquiryData(InquiryFetchDto input)
        {
            try
            {

                var result = await new BookingDAL(db, _config).GetInquiryData(input);
                if (result != null)
                {
                    return Json(result);
                }
                else
                {
                    return Json(new { Result = "error", Message = "Some Thing Went Wrong!" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("CheckStatusPaidorNot")]
        public async Task<IActionResult> CheckStatusPaidorNot(InquiryFetchDto input)
        {
            try
            {
                var result = await new BookingDAL(db, _config).GetInquiryData(input);

                var payitstatus = await new PayitDAL(db, _config, _httpClient).GetBillStatus(result.ReferenceNumber, result.BankAccountCode);

                if (payitstatus.message == "Paid" && result.Description == "Payit")
                {
                    await new BookingDAL(db, _config).UpdateBookingStatusToPaid(result.ReferenceNumber);
                    return Json(payitstatus);
                }
                if (payitstatus.message == "Paid" && result.Description == "Wallet")
                {
                    //write method to add amount in Customer wallet
                    ClaimsPrincipal customer = HttpContext.User;
                    string Id = customer.FindFirstValue("Id");
                    int CustomerId = int.Parse(Id);
                    var addamount = await new CustomerDAL(db, _config).AddCustomerWalletAmount(CustomerId, payitstatus.transaction_amount);
                    await new CustomerDAL(db, _config).UpdateCustomerWalletStatus(result.ReferenceNumber);
                    if (addamount.Amount != 0)
                    {
                        await new CustomerDAL(db, _config).AddCustomerWalletHistor(addamount.CustomerId, payitstatus.transaction_amount);
                    }
                    return Json(payitstatus);
                }
                else
                {
                    return Json(payitstatus);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddAmountinCustomerWallet")]
        public async Task<IActionResult> AddAmountinCustomerWallet(WalletAmountDto input)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetCustomerWalletAmountByCustomerId(input.CustomerId);

                var walletdate = DateTime.Now;
                var walletiquiry = new BookingDAL(db, _config).AddInquiryForWallet(result.WalletId, result.RefNumber, walletdate, input.Amount);
                await new CustomerDAL(db, _config).CustomerWalletPayitHistory(input.CustomerId, walletiquiry.Result.ReferenceNumber, input.Amount);
                if (result != null)
                {
                    return Json(new { Result = "success", Message = "Success!", data = walletiquiry.Result });
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

        [HttpGet]
        [Route("DeleteCustomerData")]
        public async Task<IActionResult> DeleteCustomerData(int CustomerId)
        {
            try
            {
                await new CustomerDAL(db, _config).DeleteCustomerData(CustomerId);
                return Json(new { Result = "success", Message = "Customer Delete SuccessFully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("IsCustomerAlReadyExist")]
        public async Task<IActionResult> IsCustomerAlReadyExist(string Email, string Mobile)
        {
            try
            {
                var result = await new CustomerDAL(db, _config).IsCustomerAlreadyExist(Email, Mobile);
                if (result.IsExist == true)
                {
                    return Json(new { Result = "success", Message = result.Message });
                }
                else
                {
                    return Json(new { Result = "error", Message = result.Message });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("CustomerPayitAmountStatus")]
        public async Task<IActionResult> CustomerPayitAmountStatus()
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId = int.Parse(Id);
                var result = await new CustomerDAL(db, _config).CustomerWalletPayitStatus(CustomerId);
                if (result != null)
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
        [AllowAnonymous]
        [HttpGet]
        [Route("CustomerHelpAndSupportMessageList")]
        public async Task<IActionResult> CustomerHelpAndSupportMessageList()
        {
            try
            {
                var result = await new CustomerDAL(db, _config).CustomerHelpAndSupportMessage();
                if (result.Count > 0)
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
        [Route("GetAllCustomerFireBaseNotification")]
        public async Task<IActionResult> GetAllCustomerFireBaseNotification()
        {
            try
            {
                var result = await new CustomerDAL(db, _config).GetAllCustomerFireBaseNotification();
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
        [Route("SendNotificationToCustomer")]
        public async Task<IActionResult> SendNotificationToCustomer(FirBaseNotificationDto input)
        {
            try
            {
                bool result = false;
                var list = await new CustomerDAL(db, _config).GetAllCustomerWithToken();
                foreach (var item in list)
                {
                    if (item.AndroidFcmToken != null)
                    {
                        result = new CustomerDAL(db, _config).SendNotification(item.AndroidFcmToken, input.Body, input.Title);
                    }
                    if (item.IosFcmToken != null)
                    {
                        result = new CustomerDAL(db, _config).SendNotification(item.IosFcmToken, input.Body, input.Title);
                    }

                }
                if (result == true)
                {
                    await new CustomerDAL(db, _config).AddCustomerFirBaseNotification(input.Title, input.Body);
                    return Json(new { Result = "success", Message = "Notification Send To Customer Successfully!" });
                }
                else
                {
                    return Json(new { Result = "error", Message = "Notification Not Send to Customers!" });
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
