using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Models;

namespace QuizService.Data.Configurations
{
    public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
    {
        public void Configure(EntityTypeBuilder<Attempt> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(x => x.Score)
                   .IsRequired();

            builder.Property(x => x.CorrectAnsNum)
                   .IsRequired();

            builder.Property(x => x.CorrectAnsPercentage)
                   .IsRequired();

            builder.Property(x => x.StartedAt)
                   .IsRequired();

            builder.Property(x => x.FinishedAt)
                   .IsRequired();

            builder.Property(x => x.DurationSeconds)
                   .IsRequired();

            // PlayerId je samo običan broj (nema foreign key ka User servisu)
            builder.Property(x => x.PlayerId)
                   .IsRequired();

            builder.Property(x => x.QuizId)
                   .IsRequired();

            // Ova veza ostaje jer je Quiz deo istog servisa
            builder.HasOne(a => a.Quiz)
                   .WithMany(q => q.Attempts)
                   .HasForeignKey(a => a.QuizId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
