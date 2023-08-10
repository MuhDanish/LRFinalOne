using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftyRoomsModel.Common
{
    public abstract class CommonProperties
    {
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }

    public enum BookingStatus
    {
        Active = 1,
        Booked = 2,
        Past = 3,
        Rejected = 4,
    }

    public enum WalletType
    {
        Credit = 1,
        debit = 2,
    }

    public enum CheckPaid
    {
        None = 1,
        UnPaid = 2,
        Paid = 3,
    }

    public enum PayitStatus
    {
        UnPaid = 1,
        Paid = 2,
    }

}
