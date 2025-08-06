using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRequirementDocuments_TenantId",
                table: "BusinessRequirementDocuments",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_ApproverId",
                table: "BusinessRequirementDocuments",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessRequirementDocuments_Users_AuthorId",
                table: "BusinessRequirementDocuments",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
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

            migrationBuilder.DropIndex(
                name: "IX_BusinessRequirementDocuments_TenantId",
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
