using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addGoalsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpentAmount",
                table: "Budgets",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Budgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Notify",
                table: "Budgets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalSpentAmount",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoalAmount = table.Column<int>(type: "int", nullable: false),
                    TotalSavedAmount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Notify",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "TotalSpentAmount",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Budgets",
                newName: "SpentAmount");
        }
    }
}
