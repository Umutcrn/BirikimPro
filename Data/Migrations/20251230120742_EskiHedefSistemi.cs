using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirikimPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class EskiHedefSistemi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HedefTutar",
                table: "Hedefler",
                newName: "Fiyat");

            migrationBuilder.RenameColumn(
                name: "BirikenTutar",
                table: "Hedefler",
                newName: "Biriken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fiyat",
                table: "Hedefler",
                newName: "HedefTutar");

            migrationBuilder.RenameColumn(
                name: "Biriken",
                table: "Hedefler",
                newName: "BirikenTutar");
        }
    }
}
