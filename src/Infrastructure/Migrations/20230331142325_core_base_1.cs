using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdaManager.Infrastructure.Migrations
{
    public partial class core_base_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_BaseDocs_NextDocId",
                table: "BaseEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntities_BaseDocs_PrevDocId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntities_NextDocId",
                table: "BaseEntities");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntities_PrevDocId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "NextDocId",
                table: "BaseEntities");

            migrationBuilder.DropColumn(
                name: "PrevDocId",
                table: "BaseEntities");

            migrationBuilder.AddColumn<int>(
                name: "NextDocId",
                table: "BaseDocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrevDocId",
                table: "BaseDocs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseDocs_PrevDocId",
                table: "BaseDocs",
                column: "PrevDocId",
                unique: true,
                filter: "[PrevDocId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseDocs_BaseDocs_PrevDocId",
                table: "BaseDocs",
                column: "PrevDocId",
                principalTable: "BaseDocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseDocs_BaseDocs_PrevDocId",
                table: "BaseDocs");

            migrationBuilder.DropIndex(
                name: "IX_BaseDocs_PrevDocId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "NextDocId",
                table: "BaseDocs");

            migrationBuilder.DropColumn(
                name: "PrevDocId",
                table: "BaseDocs");

            migrationBuilder.AddColumn<int>(
                name: "NextDocId",
                table: "BaseEntities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrevDocId",
                table: "BaseEntities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_NextDocId",
                table: "BaseEntities",
                column: "NextDocId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_PrevDocId",
                table: "BaseEntities",
                column: "PrevDocId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_BaseDocs_NextDocId",
                table: "BaseEntities",
                column: "NextDocId",
                principalTable: "BaseDocs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntities_BaseDocs_PrevDocId",
                table: "BaseEntities",
                column: "PrevDocId",
                principalTable: "BaseDocs",
                principalColumn: "Id");
        }
    }
}
