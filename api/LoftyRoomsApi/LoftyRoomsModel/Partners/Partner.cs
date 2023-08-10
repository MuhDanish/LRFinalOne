using LoftyRoomsModel.Common;
using LoftyRoomsModel.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Partners
{
    public class Partner : CommonProperties
    {
        [Key]
        public int PartnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Phone { get; set; }
        public int RoomId { get; set; }
        public string? HotelName { get; set; }
        public DateTime DateEntry { get; set; }
        public bool AllowLogin { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? AndroidFcmToken { get; set; }
        public string? IosFcmToken { get; set; }

    }
    public class PartnerWalletAmount : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public Partner Partners { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletAnount { get; set; }
    }
    public class PartnerWalletHistory : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public Partner Partners { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public WalletType Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class PartnerNotification : CommonProperties
    {
        [Key]
        public int NotificationId { get; set; }
        public int PartnerId { get; set; }
        public Partner Partners { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
    }

    public class PartnerFireBaseNotification : CommonProperties
    {
        [Key]
        public int NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }

    }
    public class PartnerNotificationDto
    {
        public int NotificationId { get; set; }
        public int PartnerId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
    }
    public class PartnerNotificationListDto
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class PartnerWalletHistoryTotalAmountDto
    {
        public List<PartnerWalletHistoryDto> PartnerWalletHistoryDto { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class PartnerWalletHistoryDto
    {
        public string PartnerName { get; set; }
        public WalletType Type { get; set; }
        public decimal Amount { get; set; }
    }
    public class PartnerDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string AndroidFcmToken { get; set; }
        public string IosFcmToken { get; set; }

    }
    public class PartnerListDto
    {
        public int PartnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Token { get; set; }

    }

    public class PartnerRoomListDto
    {
        public int AdId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public string? AdImage1 { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
    }

    public class PartnerRoomDetailDto
    {
        public int AdId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
        public string? Description { get; set; }
        public string? HotelName { get; set; }
        public string? Location { get; set; }

    }
    public class GetPartnerRoomDetail
    {
        public PartnerRoomDetailDto RoomDetail { get; set; }
        public List<string> RoomImages { get; set; }

        public List<FaciltyDetailList> FacilityList { get; set; }
    }

    public class PartnerProfileListDto
    {
        public PartnerProfile PartnerProfile { get; set; }
        public List<PartnerReview> Review { get; set; }
        public List<PartnerAdsDto> PartnerAds { get; set; }
    }

    public class PartnerAdsDto
    {
        public int AdId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
        //public string? HotelName { get; set; }
        public string? AdImage1 { get; set; }
    }


    public class PartnerProfile
    {
        public int PartnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? AdId { get; set; }
        public string? HotelName { get; set; }
    }

    public class PartnerReview
    {
        public decimal Rating { get; set; }
        public string? Review { get; set; }
        public string? ReviewName { get; set; }


    }



    public class PartnerProfileEditDto
    {
        public int PartnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
    }
    public class PartnerPasswordDto
    {
        public int PartnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
    public class PartnerChangePasswordDto
    {
        public int PartnerId { get; set; }
        public string? NewPassword { get; set; }
        public string? OldPassword { get; set; }

    }

    public class PartnerListWithTokens
    {
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string AndroidFcmToken { get; set; }
        public string IosFcmToken { get; set; }

    }
}
