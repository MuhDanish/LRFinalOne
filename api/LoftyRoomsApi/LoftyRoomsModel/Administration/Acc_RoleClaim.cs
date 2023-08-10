using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class Acc_RoleClaim
    {
        [Key]
        public int RoleClaimId { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Acc_Role? Acc_Role { get; set; }
        public int ClaimId { get; set; }
        [ForeignKey("ClaimId")]
        public Acc_Claim? Acc_Claim { get; set; }
    }
}
