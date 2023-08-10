using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Customers
{
    public class Customer : CommonProperties
    {
        [Key]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Mobile { get; set; }
        public int CutomerRoleId { get; set; }
        public string? Token { get; set; }
        public string? Claims { get; set; }
        public string? ProfileImage { get; set; }
        public string? Cnic { get; set; }
        public string? AndroidFcmToken { get; set; }
        public string? IosFcmToken { get; set; }

    }
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string? CityName { get; set; }

    }
    public class CustomerRole
    {
        [Key]
        public int CutomerRoleId { get; set; }
        public string? RoleName { get; set; }
    }

    public class CustomerHelpSupport : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customers { get; set; }
        public string? Message { get; set; }

    }

    public class CustomerRatingAndReview : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customers { get; set; }
        public string? Review { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rating { get; set; }
    }

    public class CustomerFireBaseNotification : CommonProperties
    {
        [Key]
        public int NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }

    }

    public class CustomerRatingDto
    {
        public string? Review { get; set; }
        public decimal Rating { get; set; }

    }
    public class CustomerDto
    {

        public string? Email { get; set; }
        public string? Password { get; set; }
        public string AndroidFcmToken { get; set; }
        public string IosFcmToken { get; set; }

    }

    public class CustomerListDto
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Mobile { get; set; }
        public string? ProfileImage { get; set; }

    }
    public class CustomerPasswordDto
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
    public class CustomerEditDto
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? AndroidFcmToken { get; set; }
        public string? IosFcmToken { get; set; }
        public string? ImagePath { get; set; }
        public string? Mobile { get; set; }
        public string? Cnic { get; set; }

    }


    public class FormDataModel
    {
        public string ImagePath { get; set; }
    }
    public class CityListDto
    {
        public int Id { get; set; }
        public string? CityName { get; set; }
    }
    public class RoomCityWise
    {
        public int AdId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public string? AdImage1 { get; set; }
        public decimal Price { get; set; }
        public decimal BasePrice { get; set; }
        public string? RoomType { get; set; }
    }

    public class NearByRomms
    {
        public int AdId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public string? AdImage1 { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
    }

    public class RoomPackageWise
    {
        public int AdId { get; set; }
        //public int NoOfBed { get; set; }
        //public int NoOfPerson { get; set; }
        public string? AdImage1 { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
        public string? PackageName { get; set; }
        public DateTime? PackageStartDate { get; set; }
        public DateTime? PackageEndDate { get; set; }
        public string? Description { get; set; }
    }


    public class RoomDetailDto
    {
        public int AdId { get; set; }
        public int PartnerId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public decimal Price { get; set; }
        public decimal BasePrice { get; set; }
        public string? RoomType { get; set; }
        public string? Description { get; set; }
        public string? HotelName { get; set; }
        public string? Location { get; set; }
        public bool IsFavourite { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
    public class GetRoomDetail
    {
        public RoomDetailDto RoomDetail { get; set; }
        public List<FaciltyDetailList> FacilityList { get; set; }
        public List<string> RoomImages { get; set; }
        public List<DateTime> ReservedDates { get; set; }


        //public List<string> FacilityList { get; set; }
    }
    public class RoomImages
    {
        public string? AdImage1 { get; set; }
        public string? AdImage2 { get; set; }
        public string? AdImage3 { get; set; }
        public string? AdImage4 { get; set; }
    }
    public class FaciltyDetailList
    {
        public string? FacilityName { get; set; }
        public string? Image { get; set; }
        public int Count { get; set; }
    }

    public class ChangePasswordDto
    {
        public int CustomerId { get; set; }
        public string? NewPassword { get; set; }
        public string? OldPassword { get; set; }

    }

    public class FiterRoomDto
    {
        //public List<RoomCityWise> FilterRooms { get; set; }
        public List<RoomCityWise> AllRooms { get; set; }
    }
    public class FilterRoomListDto
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime? CheckedInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string? RoomType { get; set; }
        public int PageNumber { get; set; }
        public int Rows { get; set; }
    }
    public class VoucherDto
    {
        //public int CustomerId { get; set; }
        //public decimal WalletAmount { get; set; }
        public decimal TotalAmount { get; set; }

    }
    public class VouchePriceDto
    {
        public int CustomerId { get; set; }
        public decimal UnVerifiedAmount { get; set; }

    }

    public class VoucherAmountDto
    {
        public int CustomerId { get; set; }
        public string? VoucherNumber { get; set; }
        //public decimal Amount { get; set; }
    }
    public class VoucherListDto
    {
        public int CustomerId { get; set; }
        public string? VoucherNumber { get; set; }
        public decimal Amount { get; set; }
    }


    public class WalletAmountDto
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
    }
    public class WalletListDto
    {
        public int CustomerId { get; set; }
        public int Amount { get; set; }
        public int WalletId { get; set; }
        public string RefNumber { get; set; }
    }
    public class VoucherListUnVerifiedAmountDto
    {
        public int CustomerId { get; set; }
        public string? VoucherNumber { get; set; }
        public decimal UnVerifiedAmount { get; set; }
    }

    public class CustomerExist
    {
        public string Message { get; set; }
        public bool IsExist { get; set; }
    }

    public class FireBaseNotificationListDto
    {

        public int NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class CustomerListWithTokens
    {
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string AndroidFcmToken { get; set; }
        public string IosFcmToken { get; set; }

    }
}
