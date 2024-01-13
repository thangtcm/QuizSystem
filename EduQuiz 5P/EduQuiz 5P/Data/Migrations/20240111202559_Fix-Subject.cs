using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class FixSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_AspNetUsers_UserIdRemove",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Class_ClassesId",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Subject_ClassesId",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Chapter_UserIdRemove",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "ClassesId",
                table: "Subject");

            migrationBuilder.AddColumn<int>(
                name: "ClassesId",
                table: "Chapter",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserRemoveId",
                table: "Chapter",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_ClassesId",
                table: "Chapter",
                column: "ClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_UserRemoveId",
                table: "Chapter",
                column: "UserRemoveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_AspNetUsers_UserRemoveId",
                table: "Chapter",
                column: "UserRemoveId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_Class_ClassesId",
                table: "Chapter",
                column: "ClassesId",
                principalTable: "Class",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_AspNetUsers_UserRemoveId",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_Class_ClassesId",
                table: "Chapter");

            migrationBuilder.DropIndex(
                name: "IX_Chapter_ClassesId",
                table: "Chapter");

            migrationBuilder.DropIndex(
                name: "IX_Chapter_UserRemoveId",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "ClassesId",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "UserRemoveId",
                table: "Chapter");

            migrationBuilder.AddColumn<int>(
                name: "ClassesId",
                table: "Subject",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ClassesId",
                table: "Subject",
                column: "ClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_UserIdRemove",
                table: "Chapter",
                column: "UserIdRemove");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_AspNetUsers_UserIdRemove",
                table: "Chapter",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Class_ClassesId",
                table: "Subject",
                column: "ClassesId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
