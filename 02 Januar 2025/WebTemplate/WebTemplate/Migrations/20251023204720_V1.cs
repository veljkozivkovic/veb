using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTemplate.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeIPrezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojDozvole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Modeli",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modeli", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Iznajmljivanja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikID = table.Column<int>(type: "int", nullable: false),
                    BrojDana = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iznajmljivanja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Iznajmljivanja_Korisnik_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kola",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kilometraza = table.Column<long>(type: "bigint", nullable: false),
                    Godiste = table.Column<long>(type: "bigint", nullable: false),
                    BrojSedista = table.Column<long>(type: "bigint", nullable: false),
                    CenaPoDanu = table.Column<long>(type: "bigint", nullable: false),
                    ModelID = table.Column<int>(type: "int", nullable: false),
                    IznajmljivanjeFK = table.Column<int>(type: "int", nullable: true),
                    DaLiJeIznajmljen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kola", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Kola_Iznajmljivanja_IznajmljivanjeFK",
                        column: x => x.IznajmljivanjeFK,
                        principalTable: "Iznajmljivanja",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Kola_Modeli_ModelID",
                        column: x => x.ModelID,
                        principalTable: "Modeli",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_KorisnikID",
                table: "Iznajmljivanja",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Kola_IznajmljivanjeFK",
                table: "Kola",
                column: "IznajmljivanjeFK",
                unique: true,
                filter: "[IznajmljivanjeFK] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kola_ModelID",
                table: "Kola",
                column: "ModelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kola");

            migrationBuilder.DropTable(
                name: "Iznajmljivanja");

            migrationBuilder.DropTable(
                name: "Modeli");

            migrationBuilder.DropTable(
                name: "Korisnik");
        }
    }
}
