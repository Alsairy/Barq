using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations
{
    public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.HasOne(pm => pm.User)
                   .WithMany()
                   .HasForeignKey(pm => pm.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pm => pm.Project)
                   .WithMany()
                   .HasForeignKey(pm => pm.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
