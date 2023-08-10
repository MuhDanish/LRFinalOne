using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.ModelsVM
{
    public class All_VM
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int ClaimModuleId { get; set; }
        public int ClaimId { get; set; }
        public int IsSelected { get; set; }
        public string? ModuleName { get; set; }
        public string? ClaimName { get; set; }
        public string? Claims { get; set; }
        public string? RoleName { get; set; }
        public string? Roles { get; set; }
    }
}
