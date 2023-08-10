using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class Acc_Claim
    {
        [Key]
        public int ClaimId { get; set; }
        public string? ClaimName { get; set; }
        public int ModuleId { get; set; }
    }
}
