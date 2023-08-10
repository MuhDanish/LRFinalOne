using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Common
{
    public class ResponseVM
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic SuccessList { get; set; }
        public dynamic ErrorList { get; set; }
        public ResponseVM()
        {
            SuccessList = new ExpandoObject();
            ErrorList = new ExpandoObject();
        }
    }
}
