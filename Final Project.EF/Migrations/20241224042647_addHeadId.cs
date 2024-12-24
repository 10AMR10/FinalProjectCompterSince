using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.EF.Migrations
{
    /// <inheritdoc />
    public partial class addHeadId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId1",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId1",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "Head_Of_DepartmentEmployeeId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Head_Of_DepartmentId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Head_Of_DepartmentEmployeeId",
                table: "Departments",
                column: "Head_Of_DepartmentEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments",
                column: "Head_Of_DepartmentEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Head_Of_DepartmentEmployeeId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Head_Of_DepartmentId",
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId1",
                table: "Employees",
                column: "DepartmentId1",
                unique: true,
                filter: "[DepartmentId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId1",
                table: "Employees",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }
    }
}
