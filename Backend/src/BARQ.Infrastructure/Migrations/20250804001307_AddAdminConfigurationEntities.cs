using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminConfigurationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BRDTemplateConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DocumentStructure = table.Column<string>(type: "TEXT", nullable: false),
                    SprintConfiguration = table.Column<string>(type: "TEXT", nullable: false),
                    UserStoryTemplates = table.Column<string>(type: "TEXT", nullable: false),
                    AcceptanceCriteria = table.Column<string>(type: "TEXT", nullable: false),
                    StakeholderAnalysis = table.Column<string>(type: "TEXT", nullable: false),
                    BusinessProcessModeling = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BRDTemplateConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BRDTemplateConfigurations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeGenerationConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TechnologyStack = table.Column<string>(type: "TEXT", nullable: false),
                    ArchitecturalPatterns = table.Column<string>(type: "TEXT", nullable: false),
                    CodingStandards = table.Column<string>(type: "TEXT", nullable: false),
                    QualityRequirements = table.Column<string>(type: "TEXT", nullable: false),
                    VersionRequirements = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGenerationConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGenerationConfigurations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    VisualDesignStandards = table.Column<string>(type: "TEXT", nullable: false),
                    BrandCompliance = table.Column<string>(type: "TEXT", nullable: false),
                    UIUXDesignPatterns = table.Column<string>(type: "TEXT", nullable: false),
                    ImageStyleConfiguration = table.Column<string>(type: "TEXT", nullable: false),
                    DesignSystemIntegration = table.Column<string>(type: "TEXT", nullable: false),
                    AccessibilityCompliance = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignConfigurations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PresentationConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    PresentationTemplates = table.Column<string>(type: "TEXT", nullable: false),
                    SlideStructure = table.Column<string>(type: "TEXT", nullable: false),
                    DesignBranding = table.Column<string>(type: "TEXT", nullable: false),
                    ContentDepth = table.Column<string>(type: "TEXT", nullable: false),
                    AudienceCustomization = table.Column<string>(type: "TEXT", nullable: false),
                    InteractiveElements = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationConfigurations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposalConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ProposalTemplates = table.Column<string>(type: "TEXT", nullable: false),
                    TechnicalSpecifications = table.Column<string>(type: "TEXT", nullable: false),
                    FinancialModeling = table.Column<string>(type: "TEXT", nullable: false),
                    RiskAssessment = table.Column<string>(type: "TEXT", nullable: false),
                    ImplementationPlanning = table.Column<string>(type: "TEXT", nullable: false),
                    CompetitivePositioning = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalConfigurations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestingConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    TestingMethodology = table.Column<string>(type: "TEXT", nullable: false),
                    TestCoverage = table.Column<string>(type: "TEXT", nullable: false),
                    PerformanceTesting = table.Column<string>(type: "TEXT", nullable: false),
                    SecurityTesting = table.Column<string>(type: "TEXT", nullable: false),
                    AutomatedTesting = table.Column<string>(type: "TEXT", nullable: false),
                    ComplianceTesting = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestingConfigurations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BRDTemplateConfigurations_OrganizationId",
                table: "BRDTemplateConfigurations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGenerationConfigurations_OrganizationId",
                table: "CodeGenerationConfigurations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignConfigurations_OrganizationId",
                table: "DesignConfigurations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationConfigurations_OrganizationId",
                table: "PresentationConfigurations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalConfigurations_OrganizationId",
                table: "ProposalConfigurations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingConfigurations_OrganizationId",
                table: "TestingConfigurations",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BRDTemplateConfigurations");

            migrationBuilder.DropTable(
                name: "CodeGenerationConfigurations");

            migrationBuilder.DropTable(
                name: "DesignConfigurations");

            migrationBuilder.DropTable(
                name: "PresentationConfigurations");

            migrationBuilder.DropTable(
                name: "ProposalConfigurations");

            migrationBuilder.DropTable(
                name: "TestingConfigurations");
        }
    }
}
