using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_WorkflowStep_TenantEntity_Inheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "WorkflowSteps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_TenantId",
                table: "WorkflowSteps",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkflowSteps_TenantId",
                table: "WorkflowSteps");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "WorkflowSteps");
        }
    }
}
