using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class Acc_Role
    {
        [Key]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
