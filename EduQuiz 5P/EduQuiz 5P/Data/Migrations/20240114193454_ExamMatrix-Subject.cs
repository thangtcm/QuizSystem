using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class ExamMatrixSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "ExamMatrix",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserActivityLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: true),
                    ExamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamMatrix_SubjectId",
                table: "ExamMatrix",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamMatrix_Subject_SubjectId",
                table: "ExamMatrix",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamMatrix_Subject_SubjectId",
                table: "ExamMatrix");

            migrationBuilder.DropTable(
                name: "UserActivityLog");

            migrationBuilder.DropIndex(
                name: "IX_ExamMatrix_SubjectId",
                table: "ExamMatrix");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "ExamMatrix");
        }
    }
}
