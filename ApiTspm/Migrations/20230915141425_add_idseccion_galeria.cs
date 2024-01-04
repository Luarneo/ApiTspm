using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTspm.Migrations
{
    /// <inheritdoc />
    public partial class add_idseccion_galeria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdSeccion",
                table: "Galerias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdSeccion",
                table: "Galerias");
        }
    }
}
