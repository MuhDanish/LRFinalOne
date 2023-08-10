using LoftyRoomsModel.Administration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using LoftyRoomsModel.Partners;
using LoftyRoomsModel.Setting;
using LoftyRoomsDAL.Setting;
using LoftyRoomsModel.Ads;
using LoftyRoomsModel.Customers;
using LoftyRoomsModel.Bookings;

namespace LoftyRoomsDAL.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        #region Account
        public DbSet<Acc_User> Acc_Users { get; set; }
        public DbSet<Acc_Role> Acc_Roles { get; set; }
        public DbSet<Acc_UserRole> acc_UserRoles { get; set; }
        public DbSet<Acc_Module> Acc_Modules { get; set; }
        public DbSet<Acc_Claim> Acc_Claims { get; set; }
        public DbSet<Acc_RoleClaim> Acc_RoleClaims { get; set; }
        public DbSet<AdminWalletAmount> AdminWalletAmounts { get; set; }


        #endregion
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerFireBaseNotification> PartnerFireBaseNotifications { get; set; }


        #region Setting

        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<AdType> AdTypes { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<AdFacility> AdFacilities { get; set; }
        public DbSet<FavouriteAd> FavouriteAds { get; set; }



        #endregion

        #region Customer
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerRole> CustomerRoles { get; set; }
        public DbSet<CustomerHelpSupport> CustomerHelpSupports { get; set; }
        public DbSet<CustomerRatingAndReview> CustomerRatingAndReviews { get; set; }
        public DbSet<CustomerFireBaseNotification> CustomerFireBaseNotifications { get; set; }

        public DbSet<City> Cities { get; set; }

        #endregion


        public DbSet<LoftyRoomsModel.Bookings.Booking> Bookings { get; set; }
        public DbSet<BookingNotification> BookingNotifications { get; set; }
        public DbSet<BookingRatingAndReview> BookingRatingAndReviews { get; set; }


        #region CustomerWallet
        public DbSet<CustomerWallet> CustomerWallets { get; set; }
        public DbSet<CustomerWalletAmount> CustomerWalletAmounts { get; set; }
        public DbSet<CustomerWalletHistory> CustomerWalletHistory { get; set; }
        public DbSet<CustomerWalletHistoryForPayit> CustomerWalletHistoryForPayit { get; set; }


        #endregion

        #region  PartnerWallet
        public DbSet<PartnerWalletAmount> PartnerWalletAmounts { get; set; }
        public DbSet<PartnerWalletHistory> PartnerWalletHistory { get; set; }
        public DbSet<PartnerNotification> PartnerNotifications { get; set; }


        #endregion
        public DbSet<Inquiry> Inquiries { get; set; }

        public DataTable ExecReturnQuery(string query)
        {
            using (var command = this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                this.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(result);
                    this.Database.CloseConnection();
                    return table;
                }
            }
        }
    }
}
