using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle.Infra.Migrations
{
    /// <inheritdoc />
    public partial class createmotolocated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Located",
                table: "Motocycles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Located",
                table: "Motocycles");
        }
    }
}
