using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logrus.Smith.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameOutcomeToArtifact2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_agent_outcomes_agents_agent_id",
                table: "agent_outcomes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_agent_outcomes",
                table: "agent_outcomes");

            migrationBuilder.RenameTable(
                name: "agent_outcomes",
                newName: "artifacts");

            migrationBuilder.RenameIndex(
                name: "ix_agent_outcomes_agent_id",
                table: "artifacts",
                newName: "ix_artifacts_agent_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_artifacts",
                table: "artifacts",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_artifacts_agents_agent_id",
                table: "artifacts",
                column: "agent_id",
                principalTable: "agents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_artifacts_agents_agent_id",
                table: "artifacts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_artifacts",
                table: "artifacts");

            migrationBuilder.RenameTable(
                name: "artifacts",
                newName: "agent_outcomes");

            migrationBuilder.RenameIndex(
                name: "ix_artifacts_agent_id",
                table: "agent_outcomes",
                newName: "ix_agent_outcomes_agent_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_agent_outcomes",
                table: "agent_outcomes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_agent_outcomes_agents_agent_id",
                table: "agent_outcomes",
                column: "agent_id",
                principalTable: "agents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
