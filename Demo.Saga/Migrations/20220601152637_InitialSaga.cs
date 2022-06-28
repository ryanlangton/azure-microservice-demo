using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QES.Demo.Saga.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutreachState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 256, nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    AttestationCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutreachState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "OutreachEmailAttempt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutreachStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAttempted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutreachEmailAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutreachEmailAttempt_OutreachState_OutreachStateId",
                        column: x => x.OutreachStateId,
                        principalTable: "OutreachState",
                        principalColumn: "CorrelationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutreachPhoneAttempt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutreachStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAttempted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutreachPhoneAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutreachPhoneAttempt_OutreachState_OutreachStateId",
                        column: x => x.OutreachStateId,
                        principalTable: "OutreachState",
                        principalColumn: "CorrelationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutreachEmailAttempt_OutreachStateId",
                table: "OutreachEmailAttempt",
                column: "OutreachStateId");

            migrationBuilder.CreateIndex(
                name: "IX_OutreachPhoneAttempt_OutreachStateId",
                table: "OutreachPhoneAttempt",
                column: "OutreachStateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutreachEmailAttempt");

            migrationBuilder.DropTable(
                name: "OutreachPhoneAttempt");

            migrationBuilder.DropTable(
                name: "OutreachState");
        }
    }
}
