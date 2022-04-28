using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class Changess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOnUsages_AddOns_AddOnId",
                table: "AddOnUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnUsages_Subscriptions_SubscriptionId",
                table: "AddOnUsages");

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
                name: "FK_AddOnUsages_AddOns_AddOnId",
                table: "AddOnUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOnUsages_Subscriptions_SubscriptionId",
                table: "AddOnUsages");

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
