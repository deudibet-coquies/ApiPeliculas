using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class cracioncategorias2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Categorias",
                newName: "fechaCreacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fechaCreacion",
                table: "Categorias",
                newName: "MyProperty");
        }
    }
}
