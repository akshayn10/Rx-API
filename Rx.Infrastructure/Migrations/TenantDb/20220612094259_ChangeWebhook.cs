using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rx.Infrastructure.Migrations.TenantDb
{
    public partial class ChangeWebhook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeSubscriptionWebhooks",
                columns: table => new
                {
                    WebhookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderWebhookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewSubscriptionType = table.Column<bool>(type: "bit", nullable: false),
                    NewPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RetrievedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeSubscriptionWebhooks", x => x.WebhookId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeSubscriptionWebhooks");
        }
    }
}
