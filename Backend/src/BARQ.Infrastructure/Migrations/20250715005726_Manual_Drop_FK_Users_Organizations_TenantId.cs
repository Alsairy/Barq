using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Manual_Drop_FK_Users_Organizations_TenantId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Drop any constraint specifically named FK_Users_Organizations_TenantId
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Users_Organizations_TenantId')
                BEGIN
                    ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Organizations_TenantId];
                END
                
                -- Drop all foreign key constraints from Users table that reference Organizations
                DECLARE @sql NVARCHAR(MAX) = '';
                SELECT @sql = @sql + 'ALTER TABLE [' + OBJECT_SCHEMA_NAME(fk.parent_object_id) + '].[' + OBJECT_NAME(fk.parent_object_id) + '] DROP CONSTRAINT [' + fk.name + '];' + CHAR(13)
                FROM sys.foreign_keys fk
                INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
                INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
                WHERE fk.parent_object_id = OBJECT_ID('Users')
                AND fk.referenced_object_id = OBJECT_ID('Organizations')
                AND c.name = 'TenantId';
                
                IF LEN(@sql) > 0
                BEGIN
                    EXEC sp_executesql @sql;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organizations_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
