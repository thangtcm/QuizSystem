using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class UserExamUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserExams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_UserExams_UserId",
                table: "UserExams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExams_AspNetUsers_UserId",
                table: "UserExams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExams_AspNetUsers_UserId",
                table: "UserExams");

            migrationBuilder.DropIndex(
                name: "IX_UserExams_UserId",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserExams");
        }
    }
}
