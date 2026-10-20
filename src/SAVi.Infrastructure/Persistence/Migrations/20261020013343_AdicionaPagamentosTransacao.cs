using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAVi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaPagamentosTransacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PagamentosTransacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ocorrencia = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentosTransacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentosTransacoes_Transacoes_TransacaoId",
                        column: x => x.TransacaoId,
                        principalTable: "Transacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PagamentosTransacoes_TransacaoId_Ocorrencia",
                table: "PagamentosTransacoes",
                columns: new[] { "TransacaoId", "Ocorrencia" },
                unique: true);

            // Até aqui toda despesa descontava do saldo. Para não mudar o saldo de quem já usa o app,
            // as ocorrências de despesas (Tipo = 1) com data até hoje entram como pagas; só as futuras ficam pendentes.
            // O "+ interval" do Postgres ajusta o fim do mês igual ao DateOnly.AddMonths (31/01 + 1 mês = 28/02).
            migrationBuilder.Sql("""
                INSERT INTO "PagamentosTransacoes" ("Id", "TransacaoId", "Ocorrencia", "CreatedAtUtc")
                SELECT gen_random_uuid(), t."Id", o.n, now()
                FROM "Transacoes" t
                CROSS JOIN LATERAL generate_series(1, t."MesesRecorrencia") AS o(n)
                WHERE t."Tipo" = 1
                  AND (t."Data" + make_interval(months => o.n - 1))::date <= current_date;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagamentosTransacoes");
        }
    }
}
