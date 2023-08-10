using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Setting
{
    public class Package: CommonProperties
    {
        [Key]
        public int PackageId { get; set; }
        public string? PromotionalText { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string? PromotionalImage { get; set; }

    }
}
