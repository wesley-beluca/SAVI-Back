using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAVi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaMesesRecorrenciaTransacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MesesRecorrencia",
                table: "Transacoes",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MesesRecorrencia",
                table: "Transacoes");
        }
    }
}
