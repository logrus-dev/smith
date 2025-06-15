using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logrus.Smith.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameOutcomeToArtifact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "agent_outcomes",
                newName: "outcome");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "agents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "agents");

            migrationBuilder.RenameColumn(
                name: "outcome",
                table: "agent_outcomes",
                newName: "type");
        }
    }
}
