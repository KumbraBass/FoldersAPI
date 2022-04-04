using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.FolderDb.Migrations
{
    public partial class fixRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_FoldersId1",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_FoldersId1",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FoldersId1",
                table: "Files");

            migrationBuilder.AlterColumn<long>(
                name: "FoldersId",
                table: "Files",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Files_FoldersId",
                table: "Files",
                column: "FoldersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_FoldersId",
                table: "Files",
                column: "FoldersId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_FoldersId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_FoldersId",
                table: "Files");

            migrationBuilder.AlterColumn<int>(
                name: "FoldersId",
                table: "Files",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "FoldersId1",
                table: "Files",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_FoldersId1",
                table: "Files",
                column: "FoldersId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_FoldersId1",
                table: "Files",
                column: "FoldersId1",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
