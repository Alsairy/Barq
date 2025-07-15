using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Phase3_AI_Orchestration_Workflow_Implementation_Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StepType = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Configuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputSchema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutputSchema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeoutMinutes = table.Column<int>(type: "int", nullable: true),
                    RetryConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorHandling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AllowParallelExecution = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    WorkflowTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_WorkflowSteps_ParentStepId",
                        column: x => x.ParentStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_WorkflowTemplates_WorkflowTemplateId",
                        column: x => x.WorkflowTemplateId,
                        principalTable: "WorkflowTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowDataContexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "workflow"),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "{}"),
                    DataSchema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncryptionKeyId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsEncrypted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AccessPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransformationRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    WorkflowInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkflowStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDataContexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowDataContexts_WorkflowDataContexts_ParentContextId",
                        column: x => x.ParentContextId,
                        principalTable: "WorkflowDataContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowDataContexts_WorkflowInstances_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalTable: "WorkflowInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowDataContexts_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStepExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InputData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutputData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ErrorDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMs = table.Column<long>(type: "bigint", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxRetries = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    NextRetryAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExecutionContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionLogs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceMetrics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkflowInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStepExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStepExecutions_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowStepExecutions_Users_ExecutedById",
                        column: x => x.ExecutedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowStepExecutions_WorkflowInstances_WorkflowInstanceId",
                        column: x => x.WorkflowInstanceId,
                        principalTable: "WorkflowInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowStepExecutions_WorkflowSteps_WorkflowStepId",
                        column: x => x.WorkflowStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDataContexts_IsActive",
                table: "WorkflowDataContexts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDataContexts_ParentContextId",
                table: "WorkflowDataContexts",
                column: "ParentContextId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDataContexts_Scope",
                table: "WorkflowDataContexts",
                column: "Scope");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDataContexts_TenantId",
                table: "WorkflowDataContexts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDataContexts_WorkflowInstanceId",
                table: "WorkflowDataContexts",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDataContexts_WorkflowInstanceId_Scope",
                table: "WorkflowDataContexts",
                columns: new[] { "WorkflowInstanceId", "Scope" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDataContexts_WorkflowStepId",
                table: "WorkflowDataContexts",
                column: "WorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepExecutions_AssignedToId",
                table: "WorkflowStepExecutions",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepExecutions_ExecutedById",
                table: "WorkflowStepExecutions",
                column: "ExecutedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepExecutions_Status",
                table: "WorkflowStepExecutions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepExecutions_TenantId",
                table: "WorkflowStepExecutions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepExecutions_WorkflowInstanceId",
                table: "WorkflowStepExecutions",
                column: "WorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepExecutions_WorkflowInstanceId_Status",
                table: "WorkflowStepExecutions",
                columns: new[] { "WorkflowInstanceId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStepExecutions_WorkflowStepId",
                table: "WorkflowStepExecutions",
                column: "WorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_IsActive",
                table: "WorkflowSteps",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_ParentStepId",
                table: "WorkflowSteps",
                column: "ParentStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_StepType",
                table: "WorkflowSteps",
                column: "StepType");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowTemplateId",
                table: "WorkflowSteps",
                column: "WorkflowTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowTemplateId_Order",
                table: "WorkflowSteps",
                columns: new[] { "WorkflowTemplateId", "Order" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowDataContexts");

            migrationBuilder.DropTable(
                name: "WorkflowStepExecutions");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");
        }
    }
}
