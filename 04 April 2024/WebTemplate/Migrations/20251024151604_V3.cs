using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTemplate.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sediste_Sala_SalaaID",
                table: "Sediste");

            migrationBuilder.AlterColumn<int>(
                name: "SalaaID",
                table: "Sediste",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Cena",
                table: "Sediste",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "BazicnaCena",
                table: "Sala",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "KapacitetSedista",
                table: "Sala",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Sediste_Sala_SalaaID",
                table: "Sediste",
                column: "SalaaID",
                principalTable: "Sala",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sediste_Sala_SalaaID",
                table: "Sediste");

            migrationBuilder.DropColumn(
                name: "Cena",
                table: "Sediste");

            migrationBuilder.DropColumn(
                name: "KapacitetSedista",
                table: "Sala");

            migrationBuilder.AlterColumn<int>(
                name: "SalaaID",
                table: "Sediste",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BazicnaCena",
                table: "Sala",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_Sediste_Sala_SalaaID",
                table: "Sediste",
                column: "SalaaID",
                principalTable: "Sala",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
