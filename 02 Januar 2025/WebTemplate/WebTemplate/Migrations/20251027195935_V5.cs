using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTemplate.Migrations
{
    /// <inheritdoc />
    public partial class V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_Korisnik_KorisnikID",
                table: "Iznajmljivanja");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikID",
                table: "Iznajmljivanja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_Korisnik_KorisnikID",
                table: "Iznajmljivanja",
                column: "KorisnikID",
                principalTable: "Korisnik",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iznajmljivanja_Korisnik_KorisnikID",
                table: "Iznajmljivanja");

            migrationBuilder.AlterColumn<int>(
                name: "KorisnikID",
                table: "Iznajmljivanja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Iznajmljivanja_Korisnik_KorisnikID",
                table: "Iznajmljivanja",
                column: "KorisnikID",
                principalTable: "Korisnik",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
