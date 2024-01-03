using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class UpdateExam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfQuestion = table.Column<int>(type: "int", nullable: false),
                    ExamTime = table.Column<double>(type: "float", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdCreate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdRemove = table.Column<long>(type: "bigint", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    TotalUserExam = table.Column<int>(type: "int", nullable: false),
                    ListQuestion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exam_AspNetUsers_UserIdCreate",
                        column: x => x.UserIdCreate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exam_AspNetUsers_UserIdRemove",
                        column: x => x.UserIdRemove,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExamMatrix",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameMatrix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionMatrix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpdate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdRemove = table.Column<long>(type: "bigint", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    UserRemoveId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamMatrix", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamMatrix_AspNetUsers_UserIdUpdate",
                        column: x => x.UserIdUpdate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamMatrix_AspNetUsers_UserRemoveId",
                        column: x => x.UserRemoveId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExamMatrixDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChappterId = table.Column<int>(type: "int", nullable: false),
                    NumberOfQuestion = table.Column<int>(type: "int", nullable: false),
                    ExamMatrixId = table.Column<int>(type: "int", nullable: false),
                    Component = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamMatrixDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamMatrixDetail_Chapter_ChappterId",
                        column: x => x.ChappterId,
                        principalTable: "Chapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamMatrixDetail_ExamMatrix_ExamMatrixId",
                        column: x => x.ExamMatrixId,
                        principalTable: "ExamMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exam_UserIdCreate",
                table: "Exam",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_UserIdRemove",
                table: "Exam",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_ExamMatrix_UserIdUpdate",
                table: "ExamMatrix",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_ExamMatrix_UserRemoveId",
                table: "ExamMatrix",
                column: "UserRemoveId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamMatrixDetail_ChappterId",
                table: "ExamMatrixDetail",
                column: "ChappterId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamMatrixDetail_ExamMatrixId",
                table: "ExamMatrixDetail",
                column: "ExamMatrixId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exam");

            migrationBuilder.DropTable(
                name: "ExamMatrixDetail");

            migrationBuilder.DropTable(
                name: "ExamMatrix");
        }
    }
}
