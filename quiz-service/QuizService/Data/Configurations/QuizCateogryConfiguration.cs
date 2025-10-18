using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Models;

namespace QuizService.Data.Configurations
{
    public class QuizCategoryConfiguration : IEntityTypeConfiguration<QuizCategory>
    {
        public void Configure(EntityTypeBuilder<QuizCategory> builder)
        {
            builder.HasKey(qc => new { qc.QuizId, qc.CategoryId });

            builder.HasOne(qc => qc.Quiz)
                .WithMany(q => q.QuizCategories)
                .HasForeignKey(qc => qc.QuizId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(qc => qc.Category)
                .WithMany(c => c.QuizCategories)
                .HasForeignKey(qc => qc.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(qc => qc.QuizId)
                .IsRequired();

            builder.Property(qc => qc.CategoryId)
                .IsRequired();
        }
    }
}
