using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Models;

namespace QuizService.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.TimeLimitSeconds)
                   .IsRequired();

            builder.Property(x => x.Title)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .IsRequired();

            builder.Property(x => x.QuestionsNum)
                   .IsRequired();

            // builder.Property(x => x.DifficultyLevel)
            //        .IsRequired();

            builder.Property(x => x.CreatorId)
                   .IsRequired();

            
        }
    }
}
