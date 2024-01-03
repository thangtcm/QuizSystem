using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class UpdateExamFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "Exam",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Exam",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHighSchoolExam",
                table: "Exam",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Exam",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exam_ChapterId",
                table: "Exam",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_ClassId",
                table: "Exam",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_SubjectId",
                table: "Exam",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Chapter_ChapterId",
                table: "Exam",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Class_ClassId",
                table: "Exam",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Subject_SubjectId",
                table: "Exam",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Chapter_ChapterId",
                table: "Exam");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Class_ClassId",
                table: "Exam");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Subject_SubjectId",
                table: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Exam_ChapterId",
                table: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Exam_ClassId",
                table: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Exam_SubjectId",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "IsHighSchoolExam",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Exam");
        }
    }
}
