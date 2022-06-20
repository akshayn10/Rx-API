﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rx.Infrastructure.Persistence.Context;

#nullable disable

namespace Rx.Infrastructure.Migrations.PrimaryDb
{
    [DbContext(typeof(PrimaryDbContext))]
    [Migration("20220616084440_PaymentDetails")]
    partial class PaymentDetails
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Rx.Domain.Entities.Primary.Bill", b =>
                {
                    b.Property<Guid>("BillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("BillId");

                    b.Property<string>("BillDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("GeneratedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("BillId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("OrganizationId");

                    b.Property<string>("AccountOwnerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PaymentGatewayId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMethodId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.OrganizationAddress", b =>
                {
                    b.Property<Guid>("OrganizationAddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrganizationAddressId");

                    b.HasIndex("OrganizationId")
                        .IsUnique();

                    b.ToTable("OrganizationAddresses");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.PaymentTransaction", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TransactionAmount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionStatus")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("PaymentTransactions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.SystemSubscription", b =>
                {
                    b.Property<Guid>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("SubscriptionId");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<string>("JobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("SubscriptionType")
                        .HasColumnType("bit");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("ProductPlanId");

                    b.ToTable("SystemSubscriptions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.SystemSubscriptionPlan", b =>
                {
                    b.Property<Guid>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("PlanId");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Duration")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("PlanId");

                    b.ToTable("SystemSubscriptionPlans");

                    b.HasData(
                        new
                        {
                            PlanId = new Guid("ff2c0405-fd64-4aa5-b9aa-d3cf68077f91"),
                            Description = "Basic Plan : One User(Owner)",
                            Duration = 1,
                            Name = "Basic",
                            Price = 90m
                        },
                        new
                        {
                            PlanId = new Guid("11b9c80b-86ad-4cd7-b1a7-442d2d30460c"),
                            Description = "Pro Plan : Owner, 3 Admin, 2 Finance Users",
                            Duration = 1,
                            Name = "Pro",
                            Price = 140m
                        },
                        new
                        {
                            PlanId = new Guid("4b329068-f1a3-474a-8c1b-74b182515a5e"),
                            Description = "Premium Plan : Owner, 5 Admin, 10 Finance Users",
                            Duration = 1,
                            Name = "Premium",
                            Price = 230m
                        });
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.Bill", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Primary.Organization", "Organization")
                        .WithMany("Bills")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.OrganizationAddress", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Primary.Organization", "Organization")
                        .WithOne("OrganizationAddress")
                        .HasForeignKey("Rx.Domain.Entities.Primary.OrganizationAddress", "OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.PaymentTransaction", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Primary.SystemSubscription", "SystemSubscription")
                        .WithMany("PaymentTransactions")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SystemSubscription");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.SystemSubscription", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Primary.SystemSubscriptionPlan", "SystemSubscriptionPlan")
                        .WithMany("SystemSubscriptions")
                        .HasForeignKey("ProductPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SystemSubscriptionPlan");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.Organization", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("OrganizationAddress");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.SystemSubscription", b =>
                {
                    b.Navigation("PaymentTransactions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Primary.SystemSubscriptionPlan", b =>
                {
                    b.Navigation("SystemSubscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
