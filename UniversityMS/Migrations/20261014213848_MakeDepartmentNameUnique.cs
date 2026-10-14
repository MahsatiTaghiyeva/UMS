using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityMS.Migrations
{
    /// <inheritdoc />
    public partial class MakeDepartmentNameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentName",
                table: "Departments",
                column: "DepartmentName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departments_DepartmentName",
                table: "Departments");
        }
    }
}
