using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Setting
{
    public class Facility : CommonProperties
    {
        [Key]
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public string? Image { get; set; }
        public int Count { get; set; }
    }
}
