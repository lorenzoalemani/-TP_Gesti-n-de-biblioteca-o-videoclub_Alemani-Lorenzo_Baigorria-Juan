using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alquiler_Promociones_PromocionId",
                table: "Alquiler");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Devolucion");

            migrationBuilder.AddColumn<int>(
                name: "AlquilerId",
                table: "Devolucion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PromocionId",
                table: "Alquiler",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Devolucion_AlquilerId",
                table: "Devolucion",
                column: "AlquilerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alquiler_Promociones_PromocionId",
                table: "Alquiler",
                column: "PromocionId",
                principalTable: "Promociones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Devolucion_Alquiler_AlquilerId",
                table: "Devolucion",
                column: "AlquilerId",
                principalTable: "Alquiler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alquiler_Promociones_PromocionId",
                table: "Alquiler");

            migrationBuilder.DropForeignKey(
                name: "FK_Devolucion_Alquiler_AlquilerId",
                table: "Devolucion");

            migrationBuilder.DropIndex(
                name: "IX_Devolucion_AlquilerId",
                table: "Devolucion");

            migrationBuilder.DropColumn(
                name: "AlquilerId",
                table: "Devolucion");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Devolucion",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PromocionId",
                table: "Alquiler",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alquiler_Promociones_PromocionId",
                table: "Alquiler",
                column: "PromocionId",
                principalTable: "Promociones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
