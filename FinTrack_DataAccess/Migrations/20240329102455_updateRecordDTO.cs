using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateRecordDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TakerUserName",
                table: "Transactions",
                newName: "SenderUsername");

            migrationBuilder.RenameColumn(
                name: "GiverUserName",
                table: "Transactions",
                newName: "RecieverUsername");

            migrationBuilder.AddColumn<bool>(
                name: "IsUserSender",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUserSender",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SenderUsername",
                table: "Transactions",
                newName: "TakerUserName");

            migrationBuilder.RenameColumn(
                name: "RecieverUsername",
                table: "Transactions",
                newName: "GiverUserName");
        }
    }
}
