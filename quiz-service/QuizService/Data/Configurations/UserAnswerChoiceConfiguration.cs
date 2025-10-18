using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Models;

namespace QuizService.Data.Configurations
{
    public class UserAnswerChoiceConfiguration : IEntityTypeConfiguration<UserAnswerChoice>
    {
        public void Configure(EntityTypeBuilder<UserAnswerChoice> builder)
        {
            builder.HasKey(uc => new { uc.UserAnswerId, uc.ChoiceId });

            builder.HasOne(uc => uc.UserAnswer)
                .WithMany(u => u.UserAnswerChoices)
                .HasForeignKey(uc => uc.UserAnswerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(uc => uc.Choice)
                .WithMany(c => c.UserAnswerChoices)
                .HasForeignKey(uc => uc.ChoiceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(uc => uc.UserAnswerId)
                .IsRequired();

            builder.Property(uc => uc.ChoiceId)
                .IsRequired();
        }
    }
}
