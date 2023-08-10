using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.ModelsVM
{
    public class Group_VM
    {
        public int GroupId { get; set; }
        public string? GroupRef { get; set; }
        public string? GroupName { get; set; }
        public string? PSL { get; set; }
        public string? AgreedTOBs { get; set; }
        public string? AgreedTOBsFilePath { get; set; }
        public string? HOAddress { get; set; }
        public int? CategoryId { get; set; }
        public string? BlackListedOrDNC { get; set; }
        public string? ContactHomesOrHO { get; set; }
        public string? HOPhone1 { get; set; }
        public string? HOPhone2 { get; set; }
        public string? PostCode { get; set; }
        public int? RegionId { get; set; }
        public int? AreaId { get; set; }
        public string? SingleHomeOrGroup { get; set; }
        public string? TotalGroupUnits { get; set; }
        public string? Fax { get; set; }
        public string? Website { get; set; }
        public string? GroupContactName1 { get; set; }
        public string? GroupContactName2 { get; set; }
        public string? GroupContactName3 { get; set; }
        public string? Designation1 { get; set; }
        public string? Designation2 { get; set; }
        public string? Designation3 { get; set; }
        public string? HONumber1 { get; set; }
        public string? HONumber2 { get; set; }
        public string? HONumber3 { get; set; }
        public string? HOEmail1 { get; set; }
        public string? HOEmail2 { get; set; }
        public string? HOEmail3 { get; set; }
        public string? GroupNotes { get; set; }
        public string? CategoryName { get; set; }
        public string? CityName { get; set; }
        public string? AreaName { get; set; }
    }
}
