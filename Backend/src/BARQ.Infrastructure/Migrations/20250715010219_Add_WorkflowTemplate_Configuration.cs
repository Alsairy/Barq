using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_WorkflowTemplate_Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTemplates_Organizations_OrganizationId",
                table: "WorkflowTemplates");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "WorkflowTemplates",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTemplates_IsActive",
                table: "WorkflowTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTemplates_IsDefault",
                table: "WorkflowTemplates",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTemplates_TenantId",
                table: "WorkflowTemplates",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTemplates_WorkflowType",
                table: "WorkflowTemplates",
                column: "WorkflowType");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTemplates_Organizations_OrganizationId",
                table: "WorkflowTemplates",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTemplates_Organizations_OrganizationId",
                table: "WorkflowTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowTemplates_IsActive",
                table: "WorkflowTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowTemplates_IsDefault",
                table: "WorkflowTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowTemplates_TenantId",
                table: "WorkflowTemplates");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowTemplates_WorkflowType",
                table: "WorkflowTemplates");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "WorkflowTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTemplates_Organizations_OrganizationId",
                table: "WorkflowTemplates",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
