using System;
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
                name: "Biblioteka",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAdresa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteka", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Knjiga",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GodinaIzdavanja = table.Column<int>(type: "int", nullable: false),
                    NazivIzdavaca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Broj = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knjiga", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Biblioteka_Knjiga",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BibliotekaID = table.Column<int>(type: "int", nullable: true),
                    KnjigaID = table.Column<int>(type: "int", nullable: true),
                    BrojKnjige = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteka_Knjiga", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Biblioteka_Knjiga_Biblioteka_BibliotekaID",
                        column: x => x.BibliotekaID,
                        principalTable: "Biblioteka",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Biblioteka_Knjiga_Knjiga_KnjigaID",
                        column: x => x.KnjigaID,
                        principalTable: "Knjiga",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Izdavanje",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnjigaID = table.Column<int>(type: "int", nullable: true),
                    BibliotekaID = table.Column<int>(type: "int", nullable: true),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumVracanja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izdavanje", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Izdavanje_Biblioteka_BibliotekaID",
                        column: x => x.BibliotekaID,
                        principalTable: "Biblioteka",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Izdavanje_Knjiga_KnjigaID",
                        column: x => x.KnjigaID,
                        principalTable: "Knjiga",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteka_Knjiga_BibliotekaID",
                table: "Biblioteka_Knjiga",
                column: "BibliotekaID");

            migrationBuilder.CreateIndex(
                name: "IX_Biblioteka_Knjiga_KnjigaID",
                table: "Biblioteka_Knjiga",
                column: "KnjigaID");

            migrationBuilder.CreateIndex(
                name: "IX_Izdavanje_BibliotekaID",
                table: "Izdavanje",
                column: "BibliotekaID");

            migrationBuilder.CreateIndex(
                name: "IX_Izdavanje_KnjigaID",
                table: "Izdavanje",
                column: "KnjigaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biblioteka_Knjiga");

            migrationBuilder.DropTable(
                name: "Izdavanje");

            migrationBuilder.DropTable(
                name: "Biblioteka");

            migrationBuilder.DropTable(
                name: "Knjiga");
        }
    }
}
