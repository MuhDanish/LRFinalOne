using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Setting
{
    public class Person : CommonProperties
    {
        [Key]
        public int PersonId { get; set; }
        public string? NoOfPerson { get; set; }
    }
}
