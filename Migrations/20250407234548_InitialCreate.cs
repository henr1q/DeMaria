using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeMaria.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NomePai = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NomeMae = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DataNascimentoPai = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataNascimentoMae = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CpfPai = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    CpfMae = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosCasamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataCasamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Conjuge1Id = table.Column<int>(type: "integer", nullable: false),
                    Conjuge2Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosCasamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosCasamento_Pessoas_Conjuge1Id",
                        column: x => x.Conjuge1Id,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosCasamento_Pessoas_Conjuge2Id",
                        column: x => x.Conjuge2Id,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosNascimento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegistradoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosNascimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosNascimento_Pessoas_RegistradoId",
                        column: x => x.RegistradoId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosObito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataObito = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FalecidoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosObito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosObito_Pessoas_FalecidoId",
                        column: x => x.FalecidoId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosCasamento_Conjuge1Id",
                table: "RegistrosCasamento",
                column: "Conjuge1Id");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosCasamento_Conjuge2Id",
                table: "RegistrosCasamento",
                column: "Conjuge2Id");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosNascimento_RegistradoId",
                table: "RegistrosNascimento",
                column: "RegistradoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosObito_FalecidoId",
                table: "RegistrosObito",
                column: "FalecidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosCasamento");

            migrationBuilder.DropTable(
                name: "RegistrosNascimento");

            migrationBuilder.DropTable(
                name: "RegistrosObito");

            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}
