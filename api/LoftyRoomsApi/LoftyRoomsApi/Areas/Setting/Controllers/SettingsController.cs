using LoftyRoomsApi.Common;
using LoftyRoomsDAL.Ads;
using LoftyRoomsDAL.DBContext;
using LoftyRoomsDAL.Setting;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Partners;
using LoftyRoomsModel.Setting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace LoftyRoomsApi.Areas.Setting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDBContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public SettingsController(IConfiguration config, ApplicationDBContext _db, IWebHostEnvironment hostEnvironment)
        {
            _config = config;
            db = _db;
            webHostEnvironment = hostEnvironment;
        }

        #region Facility
        [IsAuthorize("14")]
        [HttpPost]
        [Route("AddUpdateFacility")]
        public ResponseVM AddUpdateFacility()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                Facility model = new Facility();
                var Image = HttpContext.Request.Form.Files["ImagePath"];
                var ImageFilePath = new LoftyRoomsApi.Common.CommonFunctions(webHostEnvironment).UploadFacilityImage(Image);

                var data = HttpContext.Request.Form["Model"];
                model = JsonConvert.DeserializeObject<Facility>(data);
                if (ImageFilePath != "")
                {
                    model.Image = ImageFilePath;
                }
                vm = new SettingDAL(db).AddUpdateFacility(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("15")]
        [HttpGet]
        [Route("GetFacilityList")]
        public ResponseVM GetFacilityList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).GetFacilityList();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("16")]
        [HttpPost]
        [Route("DeleteFacility")]
        public ResponseVM DeleteFacility([FromBody] Facility model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).DeleteFacility(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        [HttpPost]
        [Route("GetFacilityById")]
        public ResponseVM GetFacilityById([FromBody] Facility model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).getFacilityById(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        #endregion

        #region RoomType

        [IsAuthorize("20")]
        [HttpPost]
        [Route("AddUpdateRoomType")]
        public ResponseVM AddUpdateRoomType([FromBody] Room model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).AddUpdateRoomType(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("21")]
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
        [IsAuthorize("22")]
        [HttpPost]
        [Route("DeleteRoomType")]
        public ResponseVM DeleteRoomType([FromBody] Room model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).DeleteRoomType(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        #endregion
        #region Package

        [IsAuthorize("23")]
        [HttpPost]
        [Route("AddUpdatePackage")]
        public async Task<ResponseVM> AddUpdatePackage()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                Package model = new Package();
                var Image = HttpContext.Request.Form.Files["PromotionalImagePath"];
                var PromotionalImageFilePath = new LoftyRoomsApi.Common.CommonFunctions(webHostEnvironment).UploadPromotionalImage(Image);


                var data = HttpContext.Request.Form["Model"];
                model = JsonConvert.DeserializeObject<Package>(data);

                if (PromotionalImageFilePath != "")
                {
                    model.PromotionalImage = PromotionalImageFilePath;
                }

                vm = await new SettingDAL(db).AddUpdatePackage(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = 0; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("24")]
        [HttpGet]
        [Route("GetPackageList")]
        public ResponseVM GetPackageList()
        {
            ResponseVM dt = new ResponseVM();
            try
            {
                dt = new SettingDAL(db).GetPackageList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return dt;
        }
        [IsAuthorize("23")]
        [HttpPost]
        [Route("GetPackageById")]
        public ResponseVM GetPackageById([FromBody] Package model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).getPackageById(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        [IsAuthorize("25")]
        [HttpPost]
        [Route("DeletePackage")]
        public ResponseVM DeletePackage([FromBody] Package model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).DeletePackage(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }

        #endregion

        #region Persons


        [IsAuthorize("26")]
        [HttpPost]
        [Route("AddUpdatePerson")]
        public ResponseVM AddUpdatePerson([FromBody] Person model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).AddUpdatePerson(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("27")]
        [HttpGet]
        [Route("GetPersonList")]
        public ResponseVM GetPersonList()
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).GetPersonsList();
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        [IsAuthorize("28")]
        [HttpPost]
        [Route("DeletePerson")]
        public ResponseVM DeletePerson([FromBody] Person model)
        {
            ResponseVM vm = new ResponseVM();
            try
            {
                vm = new SettingDAL(db).DeletePerson(model);
            }
            catch (Exception ex)
            {
                vm.StatusCode = -11; vm.Message = ex.Message;
            }
            return vm;
        }
        #endregion
    }
}
