using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirikimPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class AylikBirikimEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AylikBirikim",
                table: "Hedefler",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AylikBirikim",
                table: "Hedefler");
        }
    }
}
