using LoftyRoomsModel.Ads;
using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.Partners;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Bookings
{
    public class Booking : CommonProperties
    {
        [Key]
        public int BookingId { get; set; }
        public string? BookingNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public int AdId { get; set; }
        public virtual Ad Ads { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customers { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime CheckOut { get; set; }
        public BookingStatus Status { get; set; }
        public bool Paid { get; set; }
        public CheckPaid PaidStatus { get; set; }
        public bool IsRated { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CommissionPrice { get; set; }

    }
    public class BookingNotification : CommonProperties
    {
        [Key]
        public int NotificationId { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customers { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class BookRoomDetail
    {
        public int AdId { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
        public string? HotelName { get; set; }
        public string? Location { get; set; }

    }
    public class GetBookRoomDetail
    {
        public BookRoomDetail RoomDetail { get; set; }
        public List<FaciltyDetailList> FacilityList { get; set; }
        public List<string> RoomImages { get; set; }
    }

    public class BookingDateDto
    {
        public DateTime CheckedIn { get; set; }
        public DateTime CheckOut { get; set; }
    }

    public class BookingListDto
    {
        public List<BookingDto> ActiveBookings { get; set; }
        public List<BookingDto> BookedBookings { get; set; }
        public List<BookingDto> PastBookings { get; set; }
    }
    public class BookingDto
    {
        public int AdId { get; set; }
        public string? AdImage1 { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
        public string? BookingNumber { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int BookingId { get; set; }
        public int PaidStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public int Status { get; set; }
        public bool IsRated { get; set; }

    }

    public class GetBookingOutPut
    {
        public int BookingId { get; set; }
        public string? BookingNumber { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal Price { get; set; }
        public decimal CommisionPrice { get; set; }
        public int PartnerId { get; set; }
        public int AdId { get; set; }
        //public bool Paid { get; set; }

        public int PaidStatus { get; set; }

    }
    public class CreateBookingInput
    {
        public int AdId { get; set; }
        public int CustomerId { get; set; }
        public decimal Price { get; set; }
        public decimal AdminCommissionPrice { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int? Status { get; set; }
        public string? Type { get; set; }
    }

    public class BookingNotificationDto
    {
        public int NotificationId { get; set; }
        public int CustomerId { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
    public class NotificationListDto
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class PartnerBookingDto
    {
        public int BookingId { get; set; }
        public string? BookingNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime CheckOut { get; set; }
    }


    public class BookingAcceptDto

    {
        public int BookingId { get; set; }
        public bool Accept { get; set; }
    }
    public class AdminBookingDto
    {
        public int AdId { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
        public string? BookingNumber { get; set; }
        public DateTime CheckedIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int BookingId { get; set; }
        public int PaidStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public int Status { get; set; }
        public string CustomerName { get; set; }
        public string PartnerName { get; set; }
        public string HotelName { get; set; }

    }


    public class BookingRatingAndReview : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking? Bookings { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rating { get; set; }
        public string? Review { get; set; }
        public string? ReviewName { get; set; }
    }


}
