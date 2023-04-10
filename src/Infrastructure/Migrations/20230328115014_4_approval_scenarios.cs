using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class _4_approval_scenarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequest_Persons_RequesterId",
                table: "PurchaseRequest");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequest_RequesterId",
                table: "PurchaseRequest");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "RequesterId",
                table: "PurchaseRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequesterName",
                table: "PurchaseRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApprovalScenarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalScenarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    UserRoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalScenarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalSteps_ApprovalScenarios_ApprovalScenarioId",
                        column: x => x.ApprovalScenarioId,
                        principalTable: "ApprovalScenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentApprovalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseDocId = table.Column<int>(type: "int", nullable: false),
                    ApprovalStepId = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApproveDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentApprovalStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentApprovalStatuses_ApprovalSteps_ApprovalStepId",
                        column: x => x.ApprovalStepId,
                        principalTable: "ApprovalSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentApprovalStatuses_BaseDocs_BaseDocId",
                        column: x => x.BaseDocId,
                        principalTable: "BaseDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalSteps_ApprovalScenarioId",
                table: "ApprovalSteps",
                column: "ApprovalScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentApprovalStatuses_ApprovalStepId",
                table: "DocumentApprovalStatuses",
                column: "ApprovalStepId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentApprovalStatuses_BaseDocId",
                table: "DocumentApprovalStatuses",
                column: "BaseDocId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentApprovalStatuses");

            migrationBuilder.DropTable(
                name: "ApprovalSteps");

            migrationBuilder.DropTable(
                name: "ApprovalScenarios");

            migrationBuilder.DropColumn(
                name: "RequesterName",
                table: "PurchaseRequest");

            migrationBuilder.AlterColumn<int>(
                name: "RequesterId",
                table: "PurchaseRequest",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_RequesterId",
                table: "PurchaseRequest",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequest_Persons_RequesterId",
                table: "PurchaseRequest",
                column: "RequesterId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
