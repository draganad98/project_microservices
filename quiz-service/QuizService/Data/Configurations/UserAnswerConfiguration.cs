using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Models;

namespace QuizService.Data.Configurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.QuestionId)
                   .IsRequired();

            builder.Property(x => x.AttemptId)
                   .IsRequired();

            builder.Property(x => x.TextAnswer);


            builder.Property(x => x.IsCorrect);


            builder.Property(x => x.Correct);

            builder.HasOne(u => u.Question)
               .WithMany(q => q.UserAnswers)
               .HasForeignKey(u => u.QuestionId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Attempt)
               .WithMany(a => a.UserAnswers)
               .HasForeignKey(u => u.AttemptId)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
