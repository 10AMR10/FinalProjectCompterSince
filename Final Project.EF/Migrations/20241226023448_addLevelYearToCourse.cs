using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.EF.Migrations
{
    /// <inheritdoc />
    public partial class addLevelYearToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_LevelYears_levelYearId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_levelYearId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "levelYearId",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "levelYear",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "levelYear",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "levelYearId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_levelYearId",
                table: "Courses",
                column: "levelYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_LevelYears_levelYearId",
                table: "Courses",
                column: "levelYearId",
                principalTable: "LevelYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
