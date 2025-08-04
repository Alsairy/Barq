using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations
{
    public class QualityAssessmentConfiguration : IEntityTypeConfiguration<QualityAssessment>
    {
        public void Configure(EntityTypeBuilder<QualityAssessment> builder)
        {
            builder.HasOne(qa => qa.AIRequest)
                   .WithMany()
                   .HasForeignKey(qa => qa.AIRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qa => qa.Assessor)
                   .WithMany()
                   .HasForeignKey(qa => qa.AssessorId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
