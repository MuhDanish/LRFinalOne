using LoftyRoomsModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Customers
{
    public class CustomerWallet : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public string? VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customers { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletAnount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Credit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnverifiedAmount { get; set; }

    }


    public class CustomerWalletAmount : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customers { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletAnount { get; set; }
    }


    public class CustomerWalletHistoryForPayit : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customers { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public PayitStatus Status { get; set; }

    }

    public class CustomerWalletHistory : CommonProperties
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customers { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public WalletType Type { get; set; }
        public decimal Amount { get; set; }
    }
    public class CustomerWalletHistoryDto
    {
        public string CustomerName { get; set; }
        public WalletType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class GetCustomerWalletOutPut
    {
        public int CustomerId { get; set; }
        public string? VoucherNumber { get; set; }
    }


    public class GetCustomerUnVerifiedAmount
    {
        public decimal UnverifiedAmount { get; set; }
        public string? VoucherNumber { get; set; }
    }


    public class CustomerWalletPaymentAndHistory
    {
        public List<PayitWalletHistoryListDto> DepositHistoy { get; set; }
        public List<CustomerWalletHistoryDto> WalletHistory { get; set; }
    }

    public class PayitWalletHistoryListDto
    {
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public PayitStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }


    public class CustomerHelpAndSupportMessageListDto
    {
        public string CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }


    public class FirBaseNotificationDto
    {
        public string? Body { get; set; }
        public string? Title { get; set; }
    }
}
