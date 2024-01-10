using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class UpdateExamAndQuestionDifficultyLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHighSchoolExam",
                table: "Exam");

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyLevel",
                table: "Question",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ExamTime",
                table: "Exam",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DifficultyLevel",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ExamTime",
                table: "Exam",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsHighSchoolExam",
                table: "Exam",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
