using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class ConsumptionDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "AddOnWebhooks");

            migrationBuilder.RenameColumn(
                name: "ProductPlanId",
                table: "AddOnWebhooks",
                newName: "SubscriptionId");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "SubscriptionWebhooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "SubscriptionWebhooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SubscriptionType",
                table: "SubscriptionWebhooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodId",
                table: "OrganizationCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RetrievedDateTime",
                table: "AddOnWebhooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "SubscriptionWebhooks");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "OrganizationCustomers");

            migrationBuilder.DropColumn(
                name: "RetrievedDateTime",
                table: "AddOnWebhooks");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "AddOnWebhooks",
                newName: "ProductPlanId");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "SubscriptionWebhooks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "SubscriptionWebhooks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "AddOnWebhooks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
