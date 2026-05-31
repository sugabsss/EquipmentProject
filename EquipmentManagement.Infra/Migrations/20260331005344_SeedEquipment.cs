using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EquipmentManagement.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAquisicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefNumCertificado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefSetor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipaments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Equipaments",
                columns: new[] { "Id", "DataAquisicao", "Nome", "NumeroSerie", "RefNumCertificado", "RefSetor" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Multímetro Digital", "MD-0001", "CERT-001", 1 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2022, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Osciloscópio", "OSC-0002", "CERT-002", 2 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2021, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fonte de Alimentação", "FA-0003", "CERT-003", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipaments");
        }
    }
}
