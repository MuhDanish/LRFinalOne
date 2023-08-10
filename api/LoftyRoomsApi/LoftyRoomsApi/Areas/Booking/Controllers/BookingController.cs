using LoftyRoomsDAL.Booking;
using LoftyRoomsDAL.Customers;
using LoftyRoomsDAL.DBContext;
using LoftyRoomsDAL.Partners;
using LoftyRoomsModel.Bookings;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.Partners;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System.Security.Claims;
using Ubiety.Dns.Core;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LoftyRoomsApi.Areas.Booking.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private IConfiguration config;
        private ApplicationDBContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BookingController(IConfiguration _config, ApplicationDBContext _db, IWebHostEnvironment hostEnvironment)
        {
            config = _config;
            db = _db;
            webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Route("BookRoom")]
        public async Task<IActionResult> BookRoom(int AdId)
        {
            try
            {
                var result = await new BookingDAL(db, config).BookRoom(AdId);
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
        [HttpGet]
        [Route("BookRoomDetail")]
        public async Task<IActionResult> BookRoomDetail(int AdId)
        {
            try
            {
                var result = await new BookingDAL(db, config).BookRoomDetail(AdId);
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
        [Route("GetAllBooking")]
        public async Task<IActionResult> GetAllBooking()
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId = int.Parse(Id);
                var result = await new BookingDAL(db, config).GetAllBookings(CustomerId);
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
        [Route("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            try
            {
                ClaimsPrincipal customer = HttpContext.User;
                string Id = customer.FindFirstValue("Id");
                int CustomerId = int.Parse(Id);
                var result = await new BookingDAL(db, config).GetAllNotification(CustomerId);
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

        [HttpPost]
        [Route("CreateBooking")]
        public async Task<IActionResult> CreateBooking(CreateBookingInput input)
        {
            try
            {
                var result = await new BookingDAL(db, config).CreateBooking(input);

                if (result != null)
                {
                    var notificaiton = new PartnerNotificationDto
                    {
                        PartnerId = (int)result.PartnerId,
                        Message = "You have Receive a new Booking!",
                        Title = "Booking",
                    };
                    await new PartnerDAL(db, config).AddPartnerNotification(notificaiton);
                }
                return Json(new { Result = "success", Message = "Active Booking Creeated Successfully!", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("IsBookingAccept")]
        public async Task<IActionResult> IsBookingAccept(BookingAcceptDto input)
        {
            try
            {
                var result = await new BookingDAL(db, config).GetBookingById(input.BookingId);
                DateTime serverTime = DateTime.Now;
                DateTime bookingDateTime = result.BookingDate;
                TimeSpan difference = serverTime - bookingDateTime;
                if (difference.TotalMinutes > 2)
                {

                    await new BookingDAL(db, config).DeleteBooking(input.BookingId);
                    return Json(new { Result = "success", Message = "Booking Deleted Successfully!" });
                }
                else
                {
                    if (input.Accept == true)
                    {
                        await new BookingDAL(db, config).UpdateBookingPaidStatus(input.BookingId);
                        var result1 = await new BookingDAL(db, config).GetBookingById(input.BookingId);
                        return Json(new { Result = "success", Message = "Room Booked Successfully!", data = result1 });
                    }
                    else
                    {
                        await new BookingDAL(db, config).UpdateBookingStatus(input.BookingId);
                        var result2 = await new BookingDAL(db, config).GetBookingById(input.BookingId);
                        return Json(new { Result = "success", Message = "Booking Rejected!", data = result2 });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("GiveRatingAndReview")]
        public async Task<IActionResult> GiveRatingAndReview(int BookingID, decimal Rating, string? Review, string? ReviewName)
        {
            try
            {
                await new BookingDAL(db, config).GiveRatingAndReview(BookingID, Rating, Review, ReviewName);
                return Json(new { Result = "success", Message = "Rating and Review is saved!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Result = "error", Message = ex.Message });
            }
        }
    }
}
