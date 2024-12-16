using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicAndEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Units_UnitId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Employees_Head_Of_UnitEmployeeId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_Head_Of_UnitEmployeeId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UnitId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.DeleteData(
                table: "Qualities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Head_Of_UnitEmployeeId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Employees",
                newName: "DepartmentId1");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Employees",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Courses",
                newName: "PdfDescription");

            migrationBuilder.AddColumn<string>(
                name: "ArabicDescription",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicDescription",
                table: "Qualities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Qualities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicDescription",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicDescription",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArabicJob_Title",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicDescription",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ArabicLevelYear",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicTitle",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LevelYear",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UnitCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArabicTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PdfDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitCourses_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArabicJob_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Job_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitEmployees_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId1",
                table: "Employees",
                column: "DepartmentId1",
                unique: true,
                filter: "[DepartmentId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UnitCourses_UnitId",
                table: "UnitCourses",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitEmployees_UnitId",
                table: "UnitEmployees",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId1",
                table: "Employees",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId1",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "UnitCourses");

            migrationBuilder.DropTable(
                name: "UnitEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId1",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ArabicDescription",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ArabicDescription",
                table: "Qualities");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Qualities");

            migrationBuilder.DropColumn(
                name: "ArabicDescription",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ArabicDescription",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ArabicJob_Title",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ArabicDescription",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ArabicLevelYear",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ArabicTitle",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LevelYear",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Employees",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "DepartmentId1",
                table: "Employees",
                newName: "UnitId");

            migrationBuilder.RenameColumn(
                name: "PdfDescription",
                table: "Courses",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "Head_Of_UnitEmployeeId",
                table: "Units",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Head_Of_DepartmentEmployeeId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Qualities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Quality 1 Description", "Quality 1 Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Units_Head_Of_UnitEmployeeId",
                table: "Units",
                column: "Head_Of_UnitEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UnitId",
                table: "Employees",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Head_Of_DepartmentEmployeeId",
                table: "Departments",
                column: "Head_Of_DepartmentEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments",
                column: "Head_Of_DepartmentEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Units_UnitId",
                table: "Employees",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Employees_Head_Of_UnitEmployeeId",
                table: "Units",
                column: "Head_Of_UnitEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }
    }
}
