using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Setting
{

    public class Room : CommonProperties
    {
        [Key]
        public int RoomId { get; set; }
        public string? RoomType { get; set; }
    }
}
