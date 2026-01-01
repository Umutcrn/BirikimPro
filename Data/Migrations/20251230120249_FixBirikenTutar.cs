using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirikimPro.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixBirikenTutar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirikmisTutar",
                table: "Hedefler",
                newName: "BirikenTutar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirikenTutar",
                table: "Hedefler",
                newName: "BirikmisTutar");
        }
    }
}
