using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Missing_Entity_Configurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_WorkflowInstances_Projects_ProjectId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_Sprints_SprintId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_UserStories_UserStoryId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_Users_CurrentAssigneeId",
                table: "WorkflowInstances");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId1",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId1",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "ProjectMembers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "BusinessRequirementDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_IsActive",
                table: "UserRoles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId1",
                table: "UserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_TenantId",
                table: "UserRoles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId1",
                table: "UserRoles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId1",
                table: "Projects",
                column: "OrganizationId1");

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
                name: "IX_ProjectMembers_ProjectId1",
                table: "ProjectMembers",
                column: "ProjectId1");

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
                name: "FK_ProjectMembers_Projects_ProjectId1",
                table: "ProjectMembers",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Organizations_OrganizationId1",
                table: "Projects",
                column: "OrganizationId1",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ProjectOwnerId",
                table: "Projects",
                column: "ProjectOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Role_RoleId1",
                table: "UserRoles",
                column: "RoleId1",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId1",
                table: "UserRoles",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_Projects_ProjectId",
                table: "WorkflowInstances",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_Sprints_SprintId",
                table: "WorkflowInstances",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_UserStories_UserStoryId",
                table: "WorkflowInstances",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_Users_CurrentAssigneeId",
                table: "WorkflowInstances",
                column: "CurrentAssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "FK_ProjectMembers_Projects_ProjectId1",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Organizations_OrganizationId1",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ProjectOwnerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Role_RoleId1",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId1",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_Projects_ProjectId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_Sprints_SprintId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_UserStories_UserStoryId",
                table: "WorkflowInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowInstances_Users_CurrentAssigneeId",
                table: "WorkflowInstances");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_IsActive",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RoleId1",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_TenantId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId1",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OrganizationId1",
                table: "Projects");

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
                name: "IX_ProjectMembers_ProjectId1",
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
                name: "RoleId1",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "OrganizationId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "BusinessRequirementDocuments");

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
                name: "FK_WorkflowInstances_Projects_ProjectId",
                table: "WorkflowInstances",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_Sprints_SprintId",
                table: "WorkflowInstances",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_UserStories_UserStoryId",
                table: "WorkflowInstances",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowInstances_Users_CurrentAssigneeId",
                table: "WorkflowInstances",
                column: "CurrentAssigneeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
