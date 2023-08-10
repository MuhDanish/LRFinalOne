using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoftyRoomsModel.Partners;
using LoftyRoomsModel.Setting;
using LoftyRoomsModel.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoftyRoomsModel.Ads
{
    public class Ad : CommonProperties
    {
        [Key]
        public int AdId { get; set; }
        public int PersonId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public decimal Price { get; set; }
        public int? RoomId { get; set; }
        public virtual Room? Rooms { get; set; }
        public int AdTypeId { get; set; }
        public string? AdImage1 { get; set; }
        public string? AdImage2 { get; set; }
        public string? AdImage3 { get; set; }
        public string? AdImage4 { get; set; }
        public int? PackageId { get; set; }
        public DateTime? PackageStartDate { get; set; }
        public DateTime? PackageEndDate { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public bool IsFavourite { get; set; } = false;
        public string? RoomNo { get; set; }
        public int? PartnerId { get; set; }
        public Partner? Partners { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

    }

    public class FavouriteAd : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int AdId { get; set; }
        public Ad? Ads { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customers { get; set; }
        public bool IsFavourite { get; set; }

    }
    public class AdFacility
    {
        [Key]
        public int Id { get; set; }
        public int AdId { get; set; }
        public int FacilityId { get; set; }
    }



    public class AdDto : CommonProperties
    {
        public int AdId { get; set; }
        public int PersonId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public int PartnerId { get; set; }
        public decimal Price { get; set; }
        public int RoomId { get; set; }
        public int AdTypeId { get; set; }
        public string? AdImage1 { get; set; }
        public string? AdImage2 { get; set; }
        public string? AdImage3 { get; set; }
        public string? AdImage4 { get; set; }
        public int? PackageId { get; set; }
        public DateTime? PackageStartDate { get; set; }
        public DateTime? PackageEndDate { get; set; }
        public string? Description { get; set; }
        public List<int> Facilities { get; set; }
        public string? Location { get; set; }
        public string? RoomNo { get; set; }
        public decimal BasePrice { get; set; }
    }
    public class AdListDto
    {
        public int AdId { get; set; }
        public int NoOfBed { get; set; }
        public int NoOfPerson { get; set; }
        public decimal Price { get; set; }
        public string? RoomType { get; set; }
        public string? PartnerName { get; set; }
        public string? HotelName { get; set; }
        public string? Description { get; set; }

    }
}
