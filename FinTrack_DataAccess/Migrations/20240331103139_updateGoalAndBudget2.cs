using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateGoalAndBudget2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalAmount",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "BudgetAmount",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "TotalSavedAmount",
                table: "Goals",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "TotalSpentAmount",
                table: "Budgets",
                newName: "Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Goals",
                newName: "TotalSavedAmount");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Budgets",
                newName: "TotalSpentAmount");

            migrationBuilder.AddColumn<int>(
                name: "GoalAmount",
                table: "Goals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BudgetAmount",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
