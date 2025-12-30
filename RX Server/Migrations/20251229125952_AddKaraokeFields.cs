using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RX_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddKaraokeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstrumentUrl",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lyrics",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VocalUrl",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstrumentUrl",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Lyrics",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "VocalUrl",
                table: "Songs");
        }
    }
}
