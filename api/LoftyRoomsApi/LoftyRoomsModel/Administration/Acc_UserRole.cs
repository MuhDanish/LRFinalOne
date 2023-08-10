using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class Acc_UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Acc_User? Acc_User { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Acc_Role? Acc_Role { get; set; }
    }
}
