using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class UpdateExamandExamDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListQuestion",
                table: "Exam");

            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserExams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfCorrect = table.Column<int>(type: "int", nullable: false),
                    ExamTime = table.Column<int>(type: "int", nullable: false),
                    NumberOfQuestion = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KnowQuestion = table.Column<double>(type: "float", nullable: false),
                    UnderstandQuestion = table.Column<double>(type: "float", nullable: false),
                    ManipulateQuestion = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserExamDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserExamId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectAnswerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExamDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExamDetail_Answer_SelectAnswerId",
                        column: x => x.SelectAnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserExamDetail_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExamDetail_UserExams_UserExamId",
                        column: x => x.UserExamId,
                        principalTable: "UserExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_ExamId",
                table: "Question",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamDetail_QuestionId",
                table: "UserExamDetail",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamDetail_SelectAnswerId",
                table: "UserExamDetail",
                column: "SelectAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExamDetail_UserExamId",
                table: "UserExamDetail",
                column: "UserExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Exam_ExamId",
                table: "Question",
                column: "ExamId",
                principalTable: "Exam",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Exam_ExamId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "UserExamDetail");

            migrationBuilder.DropTable(
                name: "UserExams");

            migrationBuilder.DropIndex(
                name: "IX_Question_ExamId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "ListQuestion",
                table: "Exam",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
