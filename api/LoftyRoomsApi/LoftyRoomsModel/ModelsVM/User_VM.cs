using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.ModelsVM
{
    public class User_VM
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CountryName { get; set; }
        public string? RegionName { get; set; }
        public string? AreaName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int AreaId { get; set; }
    }
}
