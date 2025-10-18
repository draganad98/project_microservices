namespace QuizService.Models
{
    public class Attempt
    {
        public long Id { get; set; }
        public long Score { get; set; }
        public long CorrectAnsNum { get; set; }
        public double CorrectAnsPercentage { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public int DurationSeconds { get; set; }
        public long PlayerId { get; set; }
        public long QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

    }
}
