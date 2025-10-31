using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTemplate.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kola_Modeli_ModelID",
                table: "Kola");

            migrationBuilder.AlterColumn<int>(
                name: "ModelID",
                table: "Kola",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Kola_Modeli_ModelID",
                table: "Kola",
                column: "ModelID",
                principalTable: "Modeli",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kola_Modeli_ModelID",
                table: "Kola");

            migrationBuilder.AlterColumn<int>(
                name: "ModelID",
                table: "Kola",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Kola_Modeli_ModelID",
                table: "Kola",
                column: "ModelID",
                principalTable: "Modeli",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
