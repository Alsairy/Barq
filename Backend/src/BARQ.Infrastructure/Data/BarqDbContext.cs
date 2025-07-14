using System.Reflection;
using Microsoft.EntityFrameworkCore;
using BARQ.Core.Entities;
using BARQ.Core.Services;
using BARQ.Infrastructure.MultiTenancy;

namespace BARQ.Infrastructure.Data;

public class BarqDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public BarqDbContext(DbContextOptions<BarqDbContext> options, ITenantProvider tenantProvider) 
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<UserStory> UserStories { get; set; }
    public DbSet<WorkflowTemplate> WorkflowTemplates { get; set; }
    public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
    public DbSet<AITask> AITasks { get; set; }
    public DbSet<AIProviderConfiguration> AIProviderConfigurations { get; set; }
    public DbSet<CostTracking> CostTrackings { get; set; }
    public DbSet<ITSMTicket> ITSMTickets { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<BusinessRequirementDocument> BusinessRequirementDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BarqDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(entityType.ClrType);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    private static readonly MethodInfo SetGlobalQueryMethod = typeof(BarqDbContext)
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

    public void SetGlobalQuery<T>(ModelBuilder builder) where T : TenantEntity
    {
        builder.Entity<T>().HasQueryFilter(e => e.TenantId == _tenantProvider.GetTenantId());
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    if (entry.Entity is TenantEntity tenantEntity)
                        tenantEntity.TenantId = _tenantProvider.GetTenantId();
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
