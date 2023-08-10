using LoftyRoomsApi.Common;
using LoftyRoomsDAL.Ads;
using LoftyRoomsDAL.Booking;
using LoftyRoomsDAL.DBContext;
using LoftyRoomsDAL.Partners;
using LoftyRoomsModel.Ads;
using LoftyRoomsModel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LoftyRoomsApi.Areas.Ad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : Controller
    {
        private IConfiguration _config;
        private ApplicationDBContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AdsController(IConfiguration config, ApplicationDBContext _db, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            db = _db;
            webHostEnvironment = hostEnvironment;
        }

        [IsAuthorize("31")]
        [HttpPost]
        [Route("AddUpdateAds")]
        public ResponseVM AddUpdateAds()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                LoftyRoomsModel.Ads.AdDto model = new LoftyRoomsModel.Ads.AdDto();
                var Image = HttpContext.Request.Form.Files["AdImage1Path"];
                var AdImageFilePath = new LoftyRoomsApi.Common.CommonFunctions(webHostEnvironment).UploadAdImage1(Image);

                var Image2 = HttpContext.Request.Form.Files["AdImage2Path"];
                var AdImageFilePath2 = new LoftyRoomsApi.Common.CommonFunctions(webHostEnvironment).UploadAdImage1(Image2);

                var Image3 = HttpContext.Request.Form.Files["AdImage3Path"];
                var AdImageFilePath3 = new LoftyRoomsApi.Common.CommonFunctions(webHostEnvironment).UploadAdImage1(Image3);

                var Image4 = HttpContext.Request.Form.Files["AdImage4Path"];
                var AdImageFilePath4 = new LoftyRoomsApi.Common.CommonFunctions(webHostEnvironment).UploadAdImage1(Image4);

                var data = HttpContext.Request.Form["Model"];
                model = JsonConvert.DeserializeObject<LoftyRoomsModel.Ads.AdDto>(data);

                if (AdImageFilePath != "")
                {
                    model.AdImage1 = AdImageFilePath;
                }
                if (AdImageFilePath2 != "")
                {
                    model.AdImage2 = AdImageFilePath2;
                }
                if (AdImageFilePath3 != "")
                {
                    model.AdImage3 = AdImageFilePath3;
                }
                if (AdImageFilePath4 != "")
                {
                    model.AdImage4 = AdImageFilePath4;
                }
                vm = new AdDAL(db).AddUpdateAds(model);


            }
            catch (Exception ex)
            {
                vm.StatusCode = 0; vm.Message = ex.Message;
            }
            return vm;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAdsList")]
        public async Task<IActionResult> GetAdsList()
        {
            try
            {
                var result = await new AdDAL(db).GetAllAdsList();
                return Json(new { Result = "success", data = result });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [IsAuthorize("33")]
        [HttpPost]
        [Route("DeleteAds")]
        public ResponseVM DeleteAds([FromBody] LoftyRoomsModel.Ads.Ad model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AdDAL(db).DeleteAds(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }


        [IsAuthorize("31")]
        [HttpPost]
        [Route("GetAdById")]
        public ResponseVM GetAdById([FromBody] LoftyRoomsModel.Ads.Ad model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AdDAL(db).getAdById(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        [HttpGet]
        [Route("GetAllAdsDropDowns")]
        public ResponseVM GetAllAdsDropDowns()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new AdDAL(db).GetAdsDropDowns();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
    }
}
