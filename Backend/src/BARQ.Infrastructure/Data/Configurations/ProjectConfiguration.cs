using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasOne(p => p.ProjectOwner)
                   .WithMany()
                   .HasForeignKey(p => p.ProjectOwnerId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Organization)
                   .WithMany()
                   .HasForeignKey(p => p.OrganizationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
