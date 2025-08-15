using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLavApp.Migrations
{
    /// <inheritdoc />
    public partial class DropTabelaFechamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fechamentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fechamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    ClientesAtendidos = table.Column<int>(type: "int", nullable: false),
                    ItensProcessados = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    TotalServicos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fechamentos", x => x.Id);
                });
        }
    }
}
