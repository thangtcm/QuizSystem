using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class FixSomethingLogicName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameMatrix",
                table: "ExamMatrix",
                newName: "ExamMatrixName");

            migrationBuilder.RenameColumn(
                name: "DescriptionMatrix",
                table: "ExamMatrix",
                newName: "ExamMatrixDescription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExamMatrixName",
                table: "ExamMatrix",
                newName: "NameMatrix");

            migrationBuilder.RenameColumn(
                name: "ExamMatrixDescription",
                table: "ExamMatrix",
                newName: "DescriptionMatrix");
        }
    }
}
