using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Models;

namespace QuizService.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.QuizId)
                   .IsRequired();

            builder.Property(x => x.Type)
                   .IsRequired();

            builder.Property(x => x.Points)
                   .IsRequired();

            builder.Property(x => x.Text)
                   .IsRequired();

            builder.Property(x => x.CorrectText);

            builder.Property(x => x.Correct);

            builder.HasOne(q => q.Quiz)
               .WithMany(q => q.Questions)
               .HasForeignKey(q => q.QuizId)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
