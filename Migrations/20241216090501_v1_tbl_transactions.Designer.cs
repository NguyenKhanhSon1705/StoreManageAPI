﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoreManageAPI.Context;

#nullable disable

namespace StoreManageAPI.Migrations
{
    [DbContext(typeof(DataStore))]
    [Migration("20241216090501_v1_tbl_transactions")]
    partial class v1_tbl_transactions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("StoreManageAPI.Models.AbortedTable", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("aborted_date")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("create_table_date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("reason_abort")
                        .HasColumnType("longtext");

                    b.Property<int?>("table_id")
                        .HasColumnType("int");

                    b.Property<decimal?>("total_moneny")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("total_quantity_dish")
                        .HasColumnType("int");

                    b.Property<string>("user_id")
                        .HasColumnType("varchar(255)");

                    b.HasKey("id");

                    b.HasIndex("table_id");

                    b.HasIndex("user_id");

                    b.ToTable("AbortedTables");
                });

            modelBuilder.Entity("StoreManageAPI.Models.AbortedTableDish", b =>
                {
                    b.Property<int?>("dish_id")
                        .HasColumnType("int");

                    b.Property<int?>("aborted_table_id")
                        .HasColumnType("int");

                    b.Property<int?>("quantity")
                        .HasColumnType("int");

                    b.Property<int?>("selling_price_id")
                        .HasColumnType("int");

                    b.HasKey("dish_id", "aborted_table_id");

                    b.HasIndex("aborted_table_id");

                    b.HasIndex("selling_price_id");

                    b.ToTable("AbortedTablesDish");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Areas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AreaName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Bill", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal?>("VAT")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("payment_method")
                        .HasColumnType("int");

                    b.Property<int?>("shop_id")
                        .HasColumnType("int");

                    b.Property<int?>("table_id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("time_end")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("time_start")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("total_money")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("total_quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("user_id")
                        .HasColumnType("varchar(255)");

                    b.HasKey("id");

                    b.HasIndex("shop_id");

                    b.HasIndex("table_id");

                    b.HasIndex("user_id");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("StoreManageAPI.Models.BillDetails", b =>
                {
                    b.Property<int?>("dish_id")
                        .HasColumnType("int");

                    b.Property<int?>("bill_id")
                        .HasColumnType("int");

                    b.Property<string>("notes")
                        .HasColumnType("longtext");

                    b.Property<int?>("quantity")
                        .HasColumnType("int");

                    b.Property<int?>("selling_price_id")
                        .HasColumnType("int");

                    b.HasKey("dish_id", "bill_id");

                    b.HasIndex("bill_id");

                    b.HasIndex("selling_price_id");

                    b.ToTable("BillDetails");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("Create_At")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Dish_Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<int?>("Inventory")
                        .HasColumnType("int");

                    b.Property<bool?>("Is_Hot")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<decimal?>("Origin_Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Unit_Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Dish");
                });

            modelBuilder.Entity("StoreManageAPI.Models.DishPriceVersion", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("create_at")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("dish_id")
                        .HasColumnType("int");

                    b.Property<string>("price_version")
                        .HasColumnType("longtext");

                    b.Property<decimal?>("selling_price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool?>("status")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("update_at")
                        .HasColumnType("datetime(6)");

                    b.HasKey("id");

                    b.HasIndex("dish_id")
                        .IsUnique();

                    b.ToTable("DishPriceVersions");
                });

            modelBuilder.Entity("StoreManageAPI.Models.MenuGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("MenuGroups");
                });

            modelBuilder.Entity("StoreManageAPI.Models.MenuGroupDish", b =>
                {
                    b.Property<int>("Dish_Id")
                        .HasColumnType("int");

                    b.Property<int>("Menu_Group_Id")
                        .HasColumnType("int");

                    b.HasKey("Dish_Id", "Menu_Group_Id");

                    b.HasIndex("Menu_Group_Id");

                    b.ToTable("Menu_Groups_Dish");
                });

            modelBuilder.Entity("StoreManageAPI.Models.RefreshToken", b =>
                {
                    b.Property<string>("JwtId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DeviceInfo")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsMobile")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TokenRefresh")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("JwtId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("LockStore")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ShopAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("ShopLogo")
                        .HasColumnType("longtext");

                    b.Property<string>("ShopName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ShopPhone")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Shop");
                });

            modelBuilder.Entity("StoreManageAPI.Models.ShopUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ShopId");

                    b.HasIndex("ShopId");

                    b.ToTable("ShopUser");
                });

            modelBuilder.Entity("StoreManageAPI.Models.TableDishs", b =>
                {
                    b.Property<int?>("dishId")
                        .HasColumnType("int");

                    b.Property<int?>("tableId")
                        .HasColumnType("int");

                    b.Property<string>("notes")
                        .HasColumnType("longtext");

                    b.Property<int?>("quantity")
                        .HasColumnType("int");

                    b.Property<decimal?>("selling_Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("dishId", "tableId");

                    b.HasIndex("tableId");

                    b.ToTable("TableDishs");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Tables", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AreaId")
                        .HasColumnType("int");

                    b.Property<bool?>("HasHourlyRate")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsBooking")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("NameTable")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<decimal?>("PriceOfMunite")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("TimeEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("TimeStart")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Transactions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("bank_code")
                        .HasColumnType("longtext");

                    b.Property<int?>("bill_id")
                        .HasColumnType("int");

                    b.Property<string>("decription")
                        .HasColumnType("longtext");

                    b.Property<string>("message")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("payment_date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("payment_method")
                        .HasColumnType("longtext");

                    b.Property<string>("status_name")
                        .HasColumnType("longtext");

                    b.Property<decimal?>("total_money")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("transaction_id")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("bill_id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("StoreManageAPI.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("BirthDay")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("CodeExpireTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<bool?>("IsLock")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("IsOwner")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LockAtDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LockByUser")
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime");

                    b.Property<string>("ManagerID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Picture")
                        .HasColumnType("longtext");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("VerifiCode")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ManagerID");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("StoreManageAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("StoreManageAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("StoreManageAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StoreManageAPI.Models.AbortedTable", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Tables", "Table")
                        .WithMany()
                        .HasForeignKey("table_id");

                    b.HasOne("StoreManageAPI.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("user_id");

                    b.Navigation("Table");

                    b.Navigation("user");
                });

            modelBuilder.Entity("StoreManageAPI.Models.AbortedTableDish", b =>
                {
                    b.HasOne("StoreManageAPI.Models.AbortedTable", "abortedTable")
                        .WithMany()
                        .HasForeignKey("aborted_table_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.Dish", "dish")
                        .WithMany()
                        .HasForeignKey("dish_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.DishPriceVersion", "dishPriceVersion")
                        .WithMany()
                        .HasForeignKey("selling_price_id");

                    b.Navigation("abortedTable");

                    b.Navigation("dish");

                    b.Navigation("dishPriceVersion");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Areas", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Bill", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("shop_id");

                    b.HasOne("StoreManageAPI.Models.Tables", "Tables")
                        .WithMany()
                        .HasForeignKey("table_id");

                    b.HasOne("StoreManageAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("user_id");

                    b.Navigation("Shop");

                    b.Navigation("Tables");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StoreManageAPI.Models.BillDetails", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Bill", "Bill")
                        .WithMany()
                        .HasForeignKey("bill_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("dish_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.DishPriceVersion", "DishPriceVersion")
                        .WithMany()
                        .HasForeignKey("selling_price_id");

                    b.Navigation("Bill");

                    b.Navigation("Dish");

                    b.Navigation("DishPriceVersion");
                });

            modelBuilder.Entity("StoreManageAPI.Models.DishPriceVersion", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Dish", "dish")
                        .WithOne("Price")
                        .HasForeignKey("StoreManageAPI.Models.DishPriceVersion", "dish_id");

                    b.Navigation("dish");
                });

            modelBuilder.Entity("StoreManageAPI.Models.MenuGroup", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("StoreManageAPI.Models.MenuGroupDish", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("Dish_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.MenuGroup", "Menu_Group")
                        .WithMany()
                        .HasForeignKey("Menu_Group_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Menu_Group");
                });

            modelBuilder.Entity("StoreManageAPI.Models.RefreshToken", b =>
                {
                    b.HasOne("StoreManageAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("StoreManageAPI.Models.ShopUser", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Shop", "Shop")
                        .WithMany("StoreUsers")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.User", "User")
                        .WithMany("StoreUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StoreManageAPI.Models.TableDishs", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Dish", "dish")
                        .WithMany()
                        .HasForeignKey("dishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreManageAPI.Models.Tables", "table")
                        .WithMany()
                        .HasForeignKey("tableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("dish");

                    b.Navigation("table");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Tables", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Areas", "Areas")
                        .WithMany()
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Areas");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Transactions", b =>
                {
                    b.HasOne("StoreManageAPI.Models.Bill", "Bill")
                        .WithMany()
                        .HasForeignKey("bill_id");

                    b.Navigation("Bill");
                });

            modelBuilder.Entity("StoreManageAPI.Models.User", b =>
                {
                    b.HasOne("StoreManageAPI.Models.User", "_User")
                        .WithMany()
                        .HasForeignKey("ManagerID");

                    b.Navigation("_User");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Dish", b =>
                {
                    b.Navigation("Price");
                });

            modelBuilder.Entity("StoreManageAPI.Models.Shop", b =>
                {
                    b.Navigation("StoreUsers");
                });

            modelBuilder.Entity("StoreManageAPI.Models.User", b =>
                {
                    b.Navigation("StoreUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
