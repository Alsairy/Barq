using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BARQ.Core.Entities;

namespace BARQ.Infrastructure.Data.Configurations;

public class BusinessRequirementDocumentConfiguration : IEntityTypeConfiguration<BusinessRequirementDocument>
{
    public void Configure(EntityTypeBuilder<BusinessRequirementDocument> builder)
    {
        builder.HasKey(brd => brd.Id);

        builder.Property(brd => brd.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(brd => brd.Description)
            .HasMaxLength(2000);

        builder.Property(brd => brd.Content)
            .IsRequired();

        builder.Property(brd => brd.Version)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(brd => brd.Project)
            .WithMany()
            .HasForeignKey(brd => brd.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(brd => brd.Author)
            .WithMany()
            .HasForeignKey(brd => brd.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(brd => brd.Approver)
            .WithMany()
            .HasForeignKey(brd => brd.ApproverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(brd => brd.ProjectId);
        builder.HasIndex(brd => brd.AuthorId);
        builder.HasIndex(brd => brd.ApproverId);
    }
}
