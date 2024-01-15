using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class UpdateExam141 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnderstandQuestion",
                table: "UserExams",
                newName: "Understanding");

            migrationBuilder.RenameColumn(
                name: "ManipulateQuestion",
                table: "UserExams",
                newName: "Identification");

            migrationBuilder.RenameColumn(
                name: "KnowQuestion",
                table: "UserExams",
                newName: "Application");

            migrationBuilder.RenameColumn(
                name: "UnderstandQuestion",
                table: "Exam",
                newName: "Understanding");

            migrationBuilder.RenameColumn(
                name: "ManipulateQuestion",
                table: "Exam",
                newName: "Identification");

            migrationBuilder.RenameColumn(
                name: "KnowQuestion",
                table: "Exam",
                newName: "Application");

            migrationBuilder.AddColumn<double>(
                name: "AdvancedApplication",
                table: "UserExams",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UrlBackground",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "AdvancedApplication",
                table: "Exam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvancedApplication",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "UrlBackground",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "AdvancedApplication",
                table: "Exam");

            migrationBuilder.RenameColumn(
                name: "Understanding",
                table: "UserExams",
                newName: "UnderstandQuestion");

            migrationBuilder.RenameColumn(
                name: "Identification",
                table: "UserExams",
                newName: "ManipulateQuestion");

            migrationBuilder.RenameColumn(
                name: "Application",
                table: "UserExams",
                newName: "KnowQuestion");

            migrationBuilder.RenameColumn(
                name: "Understanding",
                table: "Exam",
                newName: "UnderstandQuestion");

            migrationBuilder.RenameColumn(
                name: "Identification",
                table: "Exam",
                newName: "ManipulateQuestion");

            migrationBuilder.RenameColumn(
                name: "Application",
                table: "Exam",
                newName: "KnowQuestion");
        }
    }
}
