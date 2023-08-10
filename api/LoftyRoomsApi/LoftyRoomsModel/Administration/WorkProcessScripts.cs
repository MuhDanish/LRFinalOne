using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class WorkProcessScripts : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public string? Remarks { get; set; }
        public string? FilePath { get; set; }
    }
}
