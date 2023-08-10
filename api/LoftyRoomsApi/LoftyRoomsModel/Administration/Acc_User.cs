using LoftyRoomsModel.Common;
using LoftyRoomsModel.Partners;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Administration
{
    public class Acc_User : CommonProperties
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Cnic { get; set; }
        public string? Mobile { get; set; }
        //public int RoleId { get; set; }
        public string? Token { get; set; }
        public string? Claims { get; set; }
        public ICollection<Acc_Claim>? Acc_Claims { get; set; }
        public ICollection<Acc_UserRole>? Acc_UserRoles { get; set; }
    }

    public class UserListDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Cnic { get; set; }
        public string? Mobile { get; set; }
        public int RoleId { get; set; }
        public string? UserType { get; set; }
        public DateTime CreatedDate { get; set; }

    }


    public class AdminWalletAmount : CommonProperties
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletAnount { get; set; }
    }
}
