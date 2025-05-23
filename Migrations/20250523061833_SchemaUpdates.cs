using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logrus.Smith.Migrations
{
    /// <inheritdoc />
    public partial class SchemaUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "input_tokens_limit",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "input_tokens_used",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "output_tokens_limit",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "output_tokens_used",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "state_json",
                table: "agents");

            migrationBuilder.AddColumn<Dictionary<string, string>>(
                name: "params",
                table: "agents",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "agents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "agent_outcomes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "agent_outcomes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "params",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "state",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "message",
                table: "agent_outcomes");

            migrationBuilder.DropColumn(
                name: "type",
                table: "agent_outcomes");

            migrationBuilder.AddColumn<int>(
                name: "input_tokens_limit",
                table: "agents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "input_tokens_used",
                table: "agents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "output_tokens_limit",
                table: "agents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "output_tokens_used",
                table: "agents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "state_json",
                table: "agents",
                type: "jsonb",
                nullable: true);
        }
    }
}
