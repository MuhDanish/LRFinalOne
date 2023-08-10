using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoftyRoomsModel.Partners;
using LoftyRoomsModel.Customers;

namespace LoftyRoomsModel.Bookings
{
    public class Inquiry
    {
        [Key]
        public int InquiryId { get; set; }
        public int? BookingId { get; set; }
        public Booking Bookings { get; set; }
        public int? CustomerWalletAmountsId { get; set; }
        public CustomerWalletAmount CustomerWalletAmounts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LateAmount { get; set; }
        public string? YearMonthFrom { get; set; }
        public string? YearMonthTo { get; set; }
        public string? Description { get; set; }
        public string? DueDate { get; set; }
        public string? VoucherValidTillDate { get; set; }
        public string? StudentIdentificationNumber { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? InstituteName { get; set; }
        public string? SessionName { get; set; }
        public string? BankAccountCode { get; set; }
        public string? CNIC { get; set; }
        public string? StudentName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? X_API_KEY { get; set; }
        public string? ReturnValue { get; set; }

    }

    public class GetInquiryOutPut
    {
        public int BookingId { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? BankAccountCode { get; set; }
        public string? X_API_KEY { get; set; }

    }

    public class GetInquiryData
    {
        public int Amount { get; set; }
        public int LateAmount { get; set; }
        public string? YearMonthFrom { get; set; }
        public string? YearMonthTo { get; set; }
        public string? Description { get; set; }
        public string? DueDate { get; set; }
        public string? VoucherValidTillDate { get; set; }
        public string? StudentIdentificationNumber { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? InstituteName { get; set; }
        public string? SessionName { get; set; }
        public string? BankAccountCode { get; set; }
        public string? CNIC { get; set; }
        public string? StudentName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? X_API_KEY { get; set; }
        public string? ReturnValue { get; set; }

    }

    public class InquiryFetchDto
    {
        public string? BankAccountCode { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? X_API_KEY { get; set; }
        //public string? Type { get; set; }
    }
    public class TransactionDto
    {
        public string reference_number { get; set; }
        public int response_code { get; set; }
        public string message { get; set; }
        public decimal transaction_amount { get; set; }
        public string transaction_date { get; set; }
        public string transaction_id { get; set; }
        public bool isSettled { get; set; }
        public string SettlementDate { get; set; }
    }
}
