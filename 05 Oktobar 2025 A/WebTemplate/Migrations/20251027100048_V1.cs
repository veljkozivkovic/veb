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
                name: "Kuca",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kuca", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kategorija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProdukcijskaKucaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Kategorija_Kuca_ProdukcijskaKucaID",
                        column: x => x.ProdukcijskaKucaID,
                        principalTable: "Kuca",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KategorijaFilmaID = table.Column<int>(type: "int", nullable: true),
                    Ocene = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Film_Kategorija_KategorijaFilmaID",
                        column: x => x.KategorijaFilmaID,
                        principalTable: "Kategorija",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Film_KategorijaFilmaID",
                table: "Film",
                column: "KategorijaFilmaID");

            migrationBuilder.CreateIndex(
                name: "IX_Kategorija_ProdukcijskaKucaID",
                table: "Kategorija",
                column: "ProdukcijskaKucaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "Kategorija");

            migrationBuilder.DropTable(
                name: "Kuca");
        }
    }
}
