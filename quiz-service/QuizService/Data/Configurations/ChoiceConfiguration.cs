using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Models;

namespace QuizService.Data.Configurations
{
    public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
    {
        public void Configure(EntityTypeBuilder<Choice> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.QuestionId)
                   .IsRequired();

            builder.Property(x => x.Text)
                   .IsRequired();

            builder.Property(x => x.IsCorrect)
                   .IsRequired();


            builder.HasOne(c => c.Question)
               .WithMany(q => q.Choices)
               .HasForeignKey(c => c.QuestionId)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
