using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Common
{
    public class DataTable_VM
    {
        public string? PageNoFrom { get; set; }
        public string? ColumnName { get; set; }
        public string? SearchString { get; set; }
        public string? Sort { get; set; }
        public string? SortBy { get; set; }
    }
}
