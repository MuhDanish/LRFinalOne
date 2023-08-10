using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Common
{
    public class Generic_VM
    {
        public int? UserId { get; set; }
        public int? CandidateId { get; set; }
        public int? RegionId { get; set; }
        public int? AreaId { get; set; }
        public int? CreatedBy { get; set; }
        public int? JobId { get; set; }
        public int? GroupId { get; set; }
        public int? JobTitleId { get; set; }
        public int? StatusId { get; set; }
        public int? Id1 { get; set; }
        public int? Id2 { get; set; }
        public int? Id3 { get; set; }
        public string? PostCode { get; set; }
        public string? JobIds { get; set; }
        public string? CandidateIdAssignedJobIds { get; set; }
        public string? String1 { get; set; }
        public string? String2 { get; set; }
        public string? String3 { get; set; }
        public string? Notes { get; set; }
        public string? Search { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsCallBack { get; set; }
        public bool? IsSendToQA { get; set; }
        public bool? IsJobNotes { get; set; }
    }
}
