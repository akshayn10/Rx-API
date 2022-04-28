using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddOnWebhooks",
                columns: table => new
                {
                    AddOnWebhookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderAddOnWebhookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOnWebhooks", x => x.AddOnWebhookId);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationCustomers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PaymentGatewayId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCustomers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebhookURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebhookSecret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreeTrialDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionWebhooks",
                columns: table => new
                {
                    WebhookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderWebhookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionWebhooks", x => x.WebhookId);
                });

            migrationBuilder.CreateTable(
                name: "AddOns",
                columns: table => new
                {
                    AddOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOns", x => x.AddOnId);
                    table.ForeignKey(
                        name: "FK_AddOns_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPlans",
                columns: table => new
                {
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPlans", x => x.PlanId);
                    table.ForeignKey(
                        name: "FK_ProductPlans_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddOnPricePerPlans",
                columns: table => new
                {
                    AddOnPricePerPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AddOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOnPricePerPlans", x => x.AddOnPricePerPlanId);
                    table.ForeignKey(
                        name: "FK_AddOnPricePerPlans_AddOns_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "AddOns",
                        principalColumn: "AddOnId",
                        onDelete:ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AddOnPricePerPlans_ProductPlans_ProductPlanId",
                        column: x => x.ProductPlanId,
                        principalTable: "ProductPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsTrial = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrganizationCustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_OrganizationCustomers_OrganizationCustomerId",
                        column: x => x.OrganizationCustomerId,
                        principalTable: "OrganizationCustomers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_ProductPlans_ProductPlanId",
                        column: x => x.ProductPlanId,
                        principalTable: "ProductPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddOnUsages",
                columns: table => new
                {
                    AddOnUsageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    AddOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOnUsages", x => x.AddOnUsageId);
                    table.ForeignKey(
                        name: "FK_AddOnUsages_AddOns_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "AddOns",
                        principalColumn: "AddOnId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AddOnUsages_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TransactionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionPaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionPaymentReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionPaymentGatewayResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionPaymentGatewayTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionPaymentGatewayTransactionAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organization",
                columns: new[] { "OrganizationId", "Description", "LogoURL", "Name", "TenantId" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Mobile Solutions provider", "www.apple.com/logo", "Apple", new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3") });

            migrationBuilder.InsertData(
                table: "OrganizationCustomers",
                columns: new[] { "CustomerId", "Email", "Name", "PaymentGatewayId" },
                values: new object[] { new Guid("be7e73a2-4dd5-4d2b-baaa-eef3a0143593"), "apple@gmail.com", "John Antony", "1234567" });

            migrationBuilder.CreateIndex(
                name: "IX_AddOnPricePerPlans_AddOnId",
                table: "AddOnPricePerPlans",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOnPricePerPlans_ProductPlanId",
                table: "AddOnPricePerPlans",
                column: "ProductPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_ProductId",
                table: "AddOns",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOnUsages_AddOnId",
                table: "AddOnUsages",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOnUsages_SubscriptionId",
                table: "AddOnUsages",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_SubscriptionId",
                table: "Bills",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_BillId",
                table: "PaymentTransactions",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPlans_ProductId",
                table: "ProductPlans",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_OrganizationCustomerId",
                table: "Subscriptions",
                column: "OrganizationCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ProductPlanId",
                table: "Subscriptions",
                column: "ProductPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddOnPricePerPlans");

            migrationBuilder.DropTable(
                name: "AddOnUsages");

            migrationBuilder.DropTable(
                name: "AddOnWebhooks");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "SubscriptionWebhooks");

            migrationBuilder.DropTable(
                name: "AddOns");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "OrganizationCustomers");

            migrationBuilder.DropTable(
                name: "ProductPlans");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
