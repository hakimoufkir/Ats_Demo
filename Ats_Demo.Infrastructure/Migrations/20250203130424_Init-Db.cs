using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ats_Demo.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Office = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Age", "CreatedDate", "LastModifiedDate", "Name", "Office", "Position", "Salary" },
                values: new object[,]
                {
                    { new Guid("09695e55-3381-4a7a-8cc6-5b61cfcaf836"), 30, new DateTime(2025, 2, 3, 14, 4, 24, 516, DateTimeKind.Local).AddTicks(554), new DateTime(2025, 2, 3, 14, 4, 24, 516, DateTimeKind.Local).AddTicks(555), "John Doe", "New York", "Software Engineer", 60000m },
                    { new Guid("55bbd9fb-0fb4-4328-97be-c3eccbf9ae49"), 28, new DateTime(2025, 2, 3, 14, 4, 24, 516, DateTimeKind.Local).AddTicks(578), new DateTime(2025, 2, 3, 14, 4, 24, 516, DateTimeKind.Local).AddTicks(579), "Michael Johnson", "San Francisco", "UI/UX Designer", 55000m },
                    { new Guid("bbd89bb9-cad4-47a0-b8dd-8422a745265c"), 35, new DateTime(2025, 2, 3, 14, 4, 24, 516, DateTimeKind.Local).AddTicks(570), new DateTime(2025, 2, 3, 14, 4, 24, 516, DateTimeKind.Local).AddTicks(571), "Jane Smith", "Los Angeles", "Project Manager", 75000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
