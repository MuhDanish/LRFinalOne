using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class Acc_Module
    {
        [Key]
        public int ModuleId { get; set; }
        public string? ModuleName { get; set; }
    }
}
