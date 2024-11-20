using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Be_Voz_Clone.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHidden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Threads",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Threads",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Threads");
        }
    }
}
