using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.EF.Migrations
{
    /// <inheritdoc />
    public partial class addLevelYearTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelYear",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "levelYearId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LevelYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelYears", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_LevelYears_levelYearId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "LevelYears");

            migrationBuilder.DropIndex(
                name: "IX_Courses_levelYearId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "levelYearId",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "LevelYear",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
