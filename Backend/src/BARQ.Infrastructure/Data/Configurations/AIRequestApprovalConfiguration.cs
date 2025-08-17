using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations
{
    public class AIRequestApprovalConfiguration : IEntityTypeConfiguration<AIRequestApproval>
    {
        public void Configure(EntityTypeBuilder<AIRequestApproval> builder)
        {
            builder.HasOne(ara => ara.AIRequest)
                   .WithMany()
                   .HasForeignKey(ara => ara.AIRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ara => ara.Approver)
                   .WithMany()
                   .HasForeignKey(ara => ara.ApproverId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ara => ara.DelegatedTo)
                   .WithMany()
                   .HasForeignKey(ara => ara.DelegatedToId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ara => ara.DelegatedFrom)
                   .WithMany()
                   .HasForeignKey(ara => ara.DelegatedFromId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
