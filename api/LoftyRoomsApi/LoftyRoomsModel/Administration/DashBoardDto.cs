using LoftyRoomsModel.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class DashBoardDto
    {

        public int ActiveCount { get; set; }
        public int BookedCount { get; set; }
        public int PastCount { get; set; }
        public int RejectCount { get; set; }

        public List<PieChartDto> PieChartData { get; set; }
        public List<AdminBookingDto> TotalBooking { get; set; }
        public List<RejectedBookingListDto> CustomerRejectedBooking { get; set; }
    }

    public class PieChartDto
    {
        public string Label { get; set; }
        public int Count { get; set; }
    }


    public class RejectedBookingInfo
    {
        public int AdId { get; set; }
        public int CustomerId { get; set; }
        public int RejectionCount { get; set; }

    }

    public class RejectedBookingListDto
    {
        public string? CustomerName { get; set; }
        public string? PartnerName { get; set; }
        public int RejectionCount { get; set; }

    }
}
