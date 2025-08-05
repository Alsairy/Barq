using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations
{
    public class BusinessRequirementDocumentConfiguration : IEntityTypeConfiguration<BusinessRequirementDocument>
    {
        public void Configure(EntityTypeBuilder<BusinessRequirementDocument> builder)
        {
            builder.HasOne(brd => brd.Author)
                   .WithMany()
                   .HasForeignKey(brd => brd.AuthorId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(brd => brd.Approver)
                   .WithMany()
                   .HasForeignKey(brd => brd.ApproverId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(brd => brd.Project)
                   .WithMany()
                   .HasForeignKey(brd => brd.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
