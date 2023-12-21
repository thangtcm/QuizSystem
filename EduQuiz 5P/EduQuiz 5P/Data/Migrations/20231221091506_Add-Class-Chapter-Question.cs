using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class AddClassChapterQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpdate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdRemove = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Class_AspNetUsers_UserIdRemove",
                        column: x => x.UserIdRemove,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Class_AspNetUsers_UserIdUpdate",
                        column: x => x.UserIdUpdate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpdate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdRemove = table.Column<long>(type: "bigint", nullable: true),
                    ClassesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subject_AspNetUsers_UserIdRemove",
                        column: x => x.UserIdRemove,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subject_AspNetUsers_UserIdUpdate",
                        column: x => x.UserIdUpdate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subject_Class_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapterDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpdate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdRemove = table.Column<long>(type: "bigint", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapter_AspNetUsers_UserIdRemove",
                        column: x => x.UserIdRemove,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chapter_AspNetUsers_UserIdUpdate",
                        column: x => x.UserIdUpdate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chapter_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LevelType = table.Column<int>(type: "int", nullable: false),
                    QuestionSolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionHints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberTimes = table.Column<int>(type: "int", nullable: false),
                    NumberCorrect = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpdate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdRemove = table.Column<long>(type: "bigint", nullable: true),
                    ChappterId = table.Column<int>(type: "int", nullable: false),
                    IsImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsImageSolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_AspNetUsers_UserIdRemove",
                        column: x => x.UserIdRemove,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_AspNetUsers_UserIdUpdate",
                        column: x => x.UserIdUpdate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Chapter_ChappterId",
                        column: x => x.ChappterId,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpdate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdRemove = table.Column<long>(type: "bigint", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_AspNetUsers_UserIdRemove",
                        column: x => x.UserIdRemove,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_AspNetUsers_UserIdUpdate",
                        column: x => x.UserIdUpdate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserIdRemove",
                table: "Answer",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserIdUpdate",
                table: "Answer",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_SubjectId",
                table: "Chapter",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_UserIdRemove",
                table: "Chapter",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_UserIdUpdate",
                table: "Chapter",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Class_UserIdRemove",
                table: "Class",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Class_UserIdUpdate",
                table: "Class",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ChappterId",
                table: "Question",
                column: "ChappterId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_UserIdRemove",
                table: "Question",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Question_UserIdUpdate",
                table: "Question",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ClassesId",
                table: "Subject",
                column: "ClassesId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_UserIdRemove",
                table: "Subject",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_UserIdUpdate",
                table: "Subject",
                column: "UserIdUpdate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Class");
        }
    }
}
