﻿// <auto-generated />
using System;
using LoftyRoomsDAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LoftyRoomsDAL.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230613064209_202306131")]
    partial class _202306131
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_Claim", b =>
                {
                    b.Property<int>("ClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("Acc_UserUserId")
                        .HasColumnType("int");

                    b.Property<string>("ClaimName")
                        .HasColumnType("longtext");

                    b.Property<int>("ModuleId")
                        .HasColumnType("int");

                    b.HasKey("ClaimId");

                    b.HasIndex("Acc_UserUserId");

                    b.ToTable("Acc_Claims");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_Module", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ModuleName")
                        .HasColumnType("longtext");

                    b.HasKey("ModuleId");

                    b.ToTable("Acc_Modules");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("longtext");

                    b.HasKey("RoleId");

                    b.ToTable("Acc_Roles");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_RoleClaim", b =>
                {
                    b.Property<int>("RoleClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClaimId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("RoleClaimId");

                    b.HasIndex("ClaimId");

                    b.HasIndex("RoleId");

                    b.ToTable("Acc_RoleClaims");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Claims")
                        .HasColumnType("longtext");

                    b.Property<string>("Cnic")
                        .HasColumnType("longtext");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("Mobile")
                        .HasColumnType("longtext");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.ToTable("Acc_Users");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_UserRole", b =>
                {
                    b.Property<int>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("acc_UserRoles");
                });

            modelBuilder.Entity("LoftyRoomsModel.Partner.Partner", b =>
                {
                    b.Property<int>("PartnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<bool>("AllowLogin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateEntry")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("HotelName")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasColumnType("longtext");

                    b.Property<string>("ZipCode")
                        .HasColumnType("longtext");

                    b.HasKey("PartnerId");

                    b.ToTable("Partners");
                });

            modelBuilder.Entity("LoftyRoomsModel.Partner.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RoomType")
                        .HasColumnType("longtext");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_Claim", b =>
                {
                    b.HasOne("LoftyRoomsModel.Administration.Acc_User", null)
                        .WithMany("Acc_Claims")
                        .HasForeignKey("Acc_UserUserId");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_RoleClaim", b =>
                {
                    b.HasOne("LoftyRoomsModel.Administration.Acc_Claim", "Acc_Claim")
                        .WithMany()
                        .HasForeignKey("ClaimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoftyRoomsModel.Administration.Acc_Role", "Acc_Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Acc_Claim");

                    b.Navigation("Acc_Role");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_UserRole", b =>
                {
                    b.HasOne("LoftyRoomsModel.Administration.Acc_Role", "Acc_Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoftyRoomsModel.Administration.Acc_User", "Acc_User")
                        .WithMany("Acc_UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Acc_Role");

                    b.Navigation("Acc_User");
                });

            modelBuilder.Entity("LoftyRoomsModel.Administration.Acc_User", b =>
                {
                    b.Navigation("Acc_Claims");

                    b.Navigation("Acc_UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
