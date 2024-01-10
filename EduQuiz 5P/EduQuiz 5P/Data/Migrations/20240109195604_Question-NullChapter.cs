using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuiz_5P.Migrations
{
    public partial class QuestionNullChapter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Chapter_ChappterId",
                table: "Question");

            migrationBuilder.AlterColumn<int>(
                name: "ChappterId",
                table: "Question",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Chapter_ChappterId",
                table: "Question",
                column: "ChappterId",
                principalTable: "Chapter",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Chapter_ChappterId",
                table: "Question");

            migrationBuilder.AlterColumn<int>(
                name: "ChappterId",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Chapter_ChappterId",
                table: "Question",
                column: "ChappterId",
                principalTable: "Chapter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
