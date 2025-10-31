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
                name: "Stan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeVlasnika = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojClanova = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stan", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Racun",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mesec = table.Column<long>(type: "bigint", nullable: false),
                    Struja = table.Column<long>(type: "bigint", nullable: true),
                    Usluge = table.Column<long>(type: "bigint", nullable: true),
                    Voda = table.Column<long>(type: "bigint", nullable: false),
                    Placen = table.Column<bool>(type: "bit", nullable: true),
                    StannID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racun", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Racun_Stan_StannID",
                        column: x => x.StannID,
                        principalTable: "Stan",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Racun_StannID",
                table: "Racun",
                column: "StannID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Racun");

            migrationBuilder.DropTable(
                name: "Stan");
        }
    }
}
