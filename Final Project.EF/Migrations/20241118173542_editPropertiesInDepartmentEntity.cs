using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.EF.Migrations
{
    /// <inheritdoc />
    public partial class editPropertiesInDepartmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Employees_Head_Of_UnitEmployeeId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "Head_Of_UnitEmployeeId",
                table: "Units",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Head_Of_DepartmentEmployeeId",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments",
                column: "Head_Of_DepartmentEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Employees_Head_Of_UnitEmployeeId",
                table: "Units",
                column: "Head_Of_UnitEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Employees_Head_Of_UnitEmployeeId",
                table: "Units");

            migrationBuilder.AlterColumn<int>(
                name: "Head_Of_UnitEmployeeId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Head_Of_DepartmentEmployeeId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments",
                column: "Head_Of_DepartmentEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Employees_Head_Of_UnitEmployeeId",
                table: "Units",
                column: "Head_Of_UnitEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
