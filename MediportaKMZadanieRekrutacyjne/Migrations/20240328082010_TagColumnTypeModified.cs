using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediportaKMZadanieRekrutacyjne.Migrations
{
    /// <inheritdoc />
    public partial class TagColumnTypeModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PopulationPercentage",
                table: "Tags",
                type: "decimal(7,5)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PopulationPercentage",
                table: "Tags",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,5)",
                oldNullable: true);
        }
    }
}
