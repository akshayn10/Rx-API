using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class CustomerTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOnPricePerPlans_AddOns_AddOnId",
                table: "AddOnPricePerPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnPricePerPlans_ProductPlans_ProductPlanId",
                table: "AddOnPricePerPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnUsages_AddOns_AddOnId",
                table: "AddOnUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnUsages_Subscriptions_SubscriptionId",
                table: "AddOnUsages");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OrganizationCustomers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionId",
                table: "AddOnUsages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddOnId",
                table: "AddOnUsages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductPlanId",
                table: "AddOnPricePerPlans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddOnId",
                table: "AddOnPricePerPlans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationCustomers_Email",
                table: "OrganizationCustomers",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnPricePerPlans_AddOns_AddOnId",
                table: "AddOnPricePerPlans",
                column: "AddOnId",
                principalTable: "AddOns",
                principalColumn: "AddOnId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnPricePerPlans_ProductPlans_ProductPlanId",
                table: "AddOnPricePerPlans",
                column: "ProductPlanId",
                principalTable: "ProductPlans",
                principalColumn: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnUsages_AddOns_AddOnId",
                table: "AddOnUsages",
                column: "AddOnId",
                principalTable: "AddOns",
                principalColumn: "AddOnId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnUsages_Subscriptions_SubscriptionId",
                table: "AddOnUsages",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOnPricePerPlans_AddOns_AddOnId",
                table: "AddOnPricePerPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnPricePerPlans_ProductPlans_ProductPlanId",
                table: "AddOnPricePerPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnUsages_AddOns_AddOnId",
                table: "AddOnUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnUsages_Subscriptions_SubscriptionId",
                table: "AddOnUsages");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationCustomers_Email",
                table: "OrganizationCustomers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OrganizationCustomers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionId",
                table: "AddOnUsages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddOnId",
                table: "AddOnUsages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductPlanId",
                table: "AddOnPricePerPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddOnId",
                table: "AddOnPricePerPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnPricePerPlans_AddOns_AddOnId",
                table: "AddOnPricePerPlans",
                column: "AddOnId",
                principalTable: "AddOns",
                principalColumn: "AddOnId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnPricePerPlans_ProductPlans_ProductPlanId",
                table: "AddOnPricePerPlans",
                column: "ProductPlanId",
                principalTable: "ProductPlans",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnUsages_AddOns_AddOnId",
                table: "AddOnUsages",
                column: "AddOnId",
                principalTable: "AddOns",
                principalColumn: "AddOnId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddOnUsages_Subscriptions_SubscriptionId",
                table: "AddOnUsages",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
