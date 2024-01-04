using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTspm.Migrations
{
    /// <inheritdoc />
    public partial class nombre_imagenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreImagen",
                table: "Secciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Galerias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombrePortada",
                table: "Anuncios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreImagen",
                table: "Secciones");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Galerias");

            migrationBuilder.DropColumn(
                name: "NombrePortada",
                table: "Anuncios");
        }
    }
}
