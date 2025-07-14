using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDeleteConflicts : Migration
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

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "BusinessRequirementDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementDocuments_ProjectId1",
                table: "BusinessRequirementDocuments",
                column: "ProjectId1");

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

            migrationBuilder.DropIndex(
                name: "IX_BusinessRequirementDocuments_ProjectId1",
                table: "BusinessRequirementDocuments");

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
        }
    }
}
