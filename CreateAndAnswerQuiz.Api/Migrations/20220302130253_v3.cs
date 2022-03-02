using Microsoft.EntityFrameworkCore.Migrations;

namespace CreateAndAnswerQuiz.Api.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizUser");

            migrationBuilder.AddColumn<bool>(
                name: "IsTrue",
                table: "Answers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrue",
                table: "Answers");

            migrationBuilder.CreateTable(
                name: "QuizUser",
                columns: table => new
                {
                    QuizzesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizUser", x => new { x.QuizzesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_QuizUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizUser_Quizzes_QuizzesId",
                        column: x => x.QuizzesId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizUser_UsersId",
                table: "QuizUser",
                column: "UsersId");
        }
    }
}
