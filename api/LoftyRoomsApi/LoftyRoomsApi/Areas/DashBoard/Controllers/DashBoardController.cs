using LoftyRoomsDAL.Booking;
using LoftyRoomsDAL.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LoftyRoomsApi.Areas.DashBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : Controller
    {
        private IConfiguration _config;
        private ApplicationDBContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public DashBoardController(IConfiguration config, ApplicationDBContext _db, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            db = _db;
            webHostEnvironment = hostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("DashBoardData")]
        public async Task<IActionResult> DashBoardData()
        {
            try
            {
                var result = await new BookingDAL(db, _config).GetDashBoardData();
                return Json(new { Result = "success", data = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
