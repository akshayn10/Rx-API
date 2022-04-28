using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationCustomer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PaymentGatewayId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCustomer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
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
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
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
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "AddOn",
                columns: table => new
                {
                    AddOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOn", x => x.AddOnId);
                    table.ForeignKey(
                        name: "FK_AddOn_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPlan",
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
                    table.PrimaryKey("PK_ProductPlan", x => x.PlanId);
                    table.ForeignKey(
                        name: "FK_ProductPlan_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
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
                    table.PrimaryKey("PK_Subscription", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscription_OrganizationCustomer_OrganizationCustomerId",
                        column: x => x.OrganizationCustomerId,
                        principalTable: "OrganizationCustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_ProductPlan_ProductPlanId",
                        column: x => x.ProductPlanId,
                        principalTable: "ProductPlan",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddOnUsage",
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
                    table.PrimaryKey("PK_AddOnUsage", x => x.AddOnUsageId);
                    table.ForeignKey(
                        name: "FK_AddOnUsage_AddOn_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "AddOn",
                        principalColumn: "AddOnId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AddOnUsage_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bill_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "SubscriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransaction",
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
                    table.PrimaryKey("PK_PaymentTransaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_PaymentTransaction_Bill_BillId",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OrganizationCustomer",
                columns: new[] { "CustomerId", "Email", "Name", "PaymentGatewayId" },
                values: new object[] { new Guid("be7e73a2-4dd5-4d2b-baaa-eef3a0143593"), "apple@gmail.com", "John Antony", "1234567" });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationId", "Description", "LogoURL", "Name", "TenantId" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Mobile Solutions provider", "www.apple.com/logo", "Apple", new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3") });

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_ProductId",
                table: "AddOn",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOnUsage_AddOnId",
                table: "AddOnUsage",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOnUsage_SubscriptionId",
                table: "AddOnUsage",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bill_SubscriptionId",
                table: "Bill",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_BillId",
                table: "PaymentTransaction",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPlan_ProductId",
                table: "ProductPlan",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_OrganizationCustomerId",
                table: "Subscription",
                column: "OrganizationCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ProductPlanId",
                table: "Subscription",
                column: "ProductPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddOnUsage");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "PaymentTransaction");

            migrationBuilder.DropTable(
                name: "AddOn");

            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "OrganizationCustomer");

            migrationBuilder.DropTable(
                name: "ProductPlan");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
