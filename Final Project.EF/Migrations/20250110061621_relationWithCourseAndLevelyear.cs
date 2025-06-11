using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.EF.Migrations
{
    /// <inheritdoc />
    public partial class relationWithCourseAndLevelyear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "levelYear",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "LevelYearId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LevelYearId",
                table: "Courses",
                column: "LevelYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_LevelYears_LevelYearId",
                table: "Courses",
                column: "LevelYearId",
                principalTable: "LevelYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_LevelYears_LevelYearId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_LevelYearId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LevelYearId",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "levelYear",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
