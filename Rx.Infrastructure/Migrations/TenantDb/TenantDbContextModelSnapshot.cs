﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rx.Infrastructure.Persistence.Context;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    [DbContext(typeof(TenantDbContext))]
    partial class TenantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Rx.Domain.Entities.Primary.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("OrganizationId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Organization");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                            Description = "Mobile Solutions provider",
                            LogoURL = "www.apple.com/logo",
                            Name = "Apple",
                            TenantId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
                        });
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOn", b =>
                {
                    b.Property<Guid>("AddOnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("AddOnId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UnitOfMeasure")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddOnId");

                    b.HasIndex("ProductId");

                    b.ToTable("AddOns");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOnPricePerPlan", b =>
                {
                    b.Property<Guid>("AddOnPricePerPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddOnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.Property<Guid?>("ProductPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AddOnPricePerPlanId");

                    b.HasIndex("AddOnId");

                    b.HasIndex("ProductPlanId");

                    b.ToTable("AddOnPricePerPlans");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOnUsage", b =>
                {
                    b.Property<Guid>("AddOnUsageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddOnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("AddOnUsageId");

                    b.HasIndex("AddOnId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("AddOnUsages");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOnWebhook", b =>
                {
                    b.Property<Guid>("AddOnWebhookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddOnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SenderAddOnWebhookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("AddOnWebhookId");

                    b.ToTable("AddOnWebhooks");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Bill", b =>
                {
                    b.Property<Guid>("BillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("BillId");

                    b.Property<DateTime>("GeneratedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SubscriptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("BillId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.OrganizationCustomer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CustomerId");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PaymentGatewayId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("OrganizationCustomers");

                    b.HasData(
                        new
                        {
                            CustomerId = new Guid("be7e73a2-4dd5-4d2b-baaa-eef3a0143593"),
                            Email = "apple@gmail.com",
                            Name = "John Antony",
                            PaymentGatewayId = "1234567"
                        });
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.PaymentTransaction", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BillId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TransactionAmount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("TransactionCurrency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionPaymentGatewayResponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionPaymentReferenceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionStatus")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionId");

                    b.HasIndex("BillId");

                    b.ToTable("PaymentTransactions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductId");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FreeTrialDays")
                        .HasColumnType("int");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RedirectURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebhookSecret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebhookURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.ProductPlan", b =>
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

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlanId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPlans");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Subscription", b =>
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

                    b.Property<bool>("IsTrial")
                        .HasColumnType("bit");

                    b.Property<Guid>("OrganizationCustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SubscriptionId");

                    b.HasIndex("OrganizationCustomerId");

                    b.HasIndex("ProductPlanId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.SubscriptionWebhook", b =>
                {
                    b.Property<Guid>("WebhookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("WebhookId");

                    b.Property<string>("CustomerEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SenderWebhookId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("WebhookId");

                    b.ToTable("SubscriptionWebhooks");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOn", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Tenant.Product", "Product")
                        .WithMany("AddOns")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOnPricePerPlan", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Tenant.AddOn", "AddOn")
                        .WithMany("AddOnPricePerPlans")
                        .HasForeignKey("AddOnId");

                    b.HasOne("Rx.Domain.Entities.Tenant.ProductPlan", "ProductPlan")
                        .WithMany("AddOnPricePerPlans")
                        .HasForeignKey("ProductPlanId");

                    b.Navigation("AddOn");

                    b.Navigation("ProductPlan");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOnUsage", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Tenant.AddOn", "AddOn")
                        .WithMany("AddOnUsages")
                        .HasForeignKey("AddOnId");

                    b.HasOne("Rx.Domain.Entities.Tenant.Subscription", "Subscription")
                        .WithMany("AddOnUsages")
                        .HasForeignKey("SubscriptionId");

                    b.Navigation("AddOn");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Bill", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Tenant.Subscription", "Subscription")
                        .WithMany("Bills")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.PaymentTransaction", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Tenant.Bill", "Bill")
                        .WithMany("PaymentTransactions")
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bill");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.ProductPlan", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Tenant.Product", "Product")
                        .WithMany("ProductPlans")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Subscription", b =>
                {
                    b.HasOne("Rx.Domain.Entities.Tenant.OrganizationCustomer", "OrganizationCustomer")
                        .WithMany("Subscriptions")
                        .HasForeignKey("OrganizationCustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rx.Domain.Entities.Tenant.ProductPlan", "ProductPlan")
                        .WithMany("Subscriptions")
                        .HasForeignKey("ProductPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrganizationCustomer");

                    b.Navigation("ProductPlan");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.AddOn", b =>
                {
                    b.Navigation("AddOnPricePerPlans");

                    b.Navigation("AddOnUsages");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Bill", b =>
                {
                    b.Navigation("PaymentTransactions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.OrganizationCustomer", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Product", b =>
                {
                    b.Navigation("AddOns");

                    b.Navigation("ProductPlans");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.ProductPlan", b =>
                {
                    b.Navigation("AddOnPricePerPlans");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("Rx.Domain.Entities.Tenant.Subscription", b =>
                {
                    b.Navigation("AddOnUsages");

                    b.Navigation("Bills");
                });
#pragma warning restore 612, 618
        }
    }
}
