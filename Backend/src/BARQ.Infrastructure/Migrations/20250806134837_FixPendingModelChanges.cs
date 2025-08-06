using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRequirementDocuments_Projects_ProjectId1",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_ApproverId",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_AuthorId",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Organizations_TenantId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ProjectOwnerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

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

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_IsActive",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_TenantId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Priority",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectKey",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Status",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_TenantId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_IsActive",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_TenantId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRequirementDocuments_Priority",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRequirementDocuments_ProjectId1",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRequirementDocuments_Status",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropIndex(
                name: "IX_BusinessRequirementDocuments_TenantId",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "BusinessRequirementDocuments");

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
                name: "FK_BusinessRequirementDocuments_Users_ApproverId",
                table: "BusinessRequirementDocuments",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_AuthorId",
                table: "BusinessRequirementDocuments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ProjectOwnerId",
                table: "Projects",
                column: "ProjectOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTemplates_Organizations_OrganizationId",
                table: "WorkflowTemplates",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_ApproverId",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_AuthorId",
                table: "BusinessRequirementDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ProjectOwnerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

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

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "BusinessRequirementDocuments",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_IsActive",
                table: "UserRoles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_TenantId",
                table: "UserRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Priority",
                table: "Projects",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectKey",
                table: "Projects",
                column: "ProjectKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Status",
                table: "Projects",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TenantId",
                table: "Projects",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_IsActive",
                table: "ProjectMembers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_TenantId",
                table: "ProjectMembers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementDocuments_Priority",
                table: "BusinessRequirementDocuments",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementDocuments_ProjectId1",
                table: "BusinessRequirementDocuments",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementDocuments_Status",
                table: "BusinessRequirementDocuments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementDocuments_TenantId",
                table: "BusinessRequirementDocuments",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRequirementDocuments_Projects_ProjectId1",
                table: "BusinessRequirementDocuments",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_ApproverId",
                table: "BusinessRequirementDocuments",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_AuthorId",
                table: "BusinessRequirementDocuments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Organizations_TenantId",
                table: "Projects",
                column: "TenantId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ProjectOwnerId",
                table: "Projects",
                column: "ProjectOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTemplates_Organizations_OrganizationId",
                table: "WorkflowTemplates",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }
    }
}
