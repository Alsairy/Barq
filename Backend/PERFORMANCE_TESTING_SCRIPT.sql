
USE BarqDb;
GO

SET STATISTICS IO ON;
SET STATISTICS TIME ON;
GO

PRINT '=== Test 1: Organization Queries with Tenant Filtering ===';

DECLARE @tenant1 UNIQUEIDENTIFIER = '476da891-1280-468e-a785-391243584bd4';
DECLARE @tenant2 UNIQUEIDENTIFIER = '91b85b71-f5ec-4f68-9a6e-29cd0e80f679';

PRINT 'Query 1a: Select organization for Tenant 1';
SELECT * FROM Organizations WHERE Id = @tenant1;

PRINT 'Query 1b: Select organization for Tenant 2';
SELECT * FROM Organizations WHERE Id = @tenant2;

PRINT '=== Test 2: User Queries with Tenant Filtering ===';

PRINT 'Query 2a: Select users for Tenant 1';
SELECT * FROM Users WHERE TenantId = @tenant1;

PRINT 'Query 2b: Select users for Tenant 2';
SELECT * FROM Users WHERE TenantId = @tenant2;

PRINT '=== Test 3: Project Queries with Tenant Filtering ===';

PRINT 'Query 3a: Select projects for Tenant 1';
SELECT * FROM Projects WHERE TenantId = @tenant1;

PRINT 'Query 3b: Select projects for Tenant 2';
SELECT * FROM Projects WHERE TenantId = @tenant2;

PRINT '=== Test 4: Complex Join Queries with Tenant Filtering ===';

PRINT 'Query 4a: Users with their projects for Tenant 1';
SELECT u.Email, u.FirstName, u.LastName, p.Name as ProjectName
FROM Users u
LEFT JOIN Projects p ON u.TenantId = p.TenantId
WHERE u.TenantId = @tenant1;

PRINT 'Query 4b: Users with their projects for Tenant 2';
SELECT u.Email, u.FirstName, u.LastName, p.Name as ProjectName
FROM Users u
LEFT JOIN Projects p ON u.TenantId = p.TenantId
WHERE u.TenantId = @tenant2;

PRINT '=== Test 5: Index Analysis ===';

SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    c.name AS ColumnName,
    i.type_desc AS IndexType
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE c.name = 'TenantId'
ORDER BY t.name, i.name;

PRINT '=== Test 6: Execution Plan Analysis ===';

SET SHOWPLAN_ALL ON;

SELECT u.Email, p.Name 
FROM Users u 
INNER JOIN Projects p ON u.TenantId = p.TenantId 
WHERE u.TenantId = @tenant1;

SET SHOWPLAN_ALL OFF;

PRINT '=== Test 7: Performance Impact Assessment ===';

PRINT 'Query without tenant filter (baseline):';
SELECT COUNT(*) FROM Users;

PRINT 'Query with tenant filter:';
SELECT COUNT(*) FROM Users WHERE TenantId = @tenant1;

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
GO

PRINT '=== Performance Testing Complete ===';
