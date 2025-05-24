using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineKitapMagazasi.API.Migrations
{
    /// <inheritdoc />
    public partial class KullaniciRolEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Kullanicilar");
        }
    }
}
