using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class AddExamType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamType",
                table: "Exam",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamType",
                table: "Exam");
        }
    }
}
