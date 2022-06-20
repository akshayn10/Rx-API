using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.PrimaryDb
{
    public partial class PlanSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemSubscriptionPlans",
                columns: new[] { "PlanId", "Description", "Duration", "Name", "Price" },
                values: new object[] { new Guid("11b9c80b-86ad-4cd7-b1a7-442d2d30460c"), "Pro Plan : Owner, 3 Admin, 2 Finance Users", 1, "Pro", 140m });

            migrationBuilder.InsertData(
                table: "SystemSubscriptionPlans",
                columns: new[] { "PlanId", "Description", "Duration", "Name", "Price" },
                values: new object[] { new Guid("4b329068-f1a3-474a-8c1b-74b182515a5e"), "Premium Plan : Owner, 5 Admin, 10 Finance Users", 1, "Premium", 230m });

            migrationBuilder.InsertData(
                table: "SystemSubscriptionPlans",
                columns: new[] { "PlanId", "Description", "Duration", "Name", "Price" },
                values: new object[] { new Guid("ff2c0405-fd64-4aa5-b9aa-d3cf68077f91"), "Basic Plan : One User(Owner)", 1, "Basic", 90m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemSubscriptionPlans",
                keyColumn: "PlanId",
                keyValue: new Guid("11b9c80b-86ad-4cd7-b1a7-442d2d30460c"));

            migrationBuilder.DeleteData(
                table: "SystemSubscriptionPlans",
                keyColumn: "PlanId",
                keyValue: new Guid("4b329068-f1a3-474a-8c1b-74b182515a5e"));

            migrationBuilder.DeleteData(
                table: "SystemSubscriptionPlans",
                keyColumn: "PlanId",
                keyValue: new Guid("ff2c0405-fd64-4aa5-b9aa-d3cf68077f91"));
        }
    }
}
