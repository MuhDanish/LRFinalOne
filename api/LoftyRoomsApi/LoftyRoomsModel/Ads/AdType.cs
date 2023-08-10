using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Ads
{
    public class AdType : CommonProperties
    {
        [Key]
        public int AdTypeId { get; set; }
        public string? AdTypeName { get; set; }
    }
}
