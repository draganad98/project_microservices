namespace QuizService.DTO.QuizDTO
{
    public class AttemptResponseDTO
    {
        public long Id { get; set; }
        public long QuizId { get; set; }
        public long PlayerId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public int DurationSeconds { get; set; }
        public long CorrectAnsNum { get; set; }
        public double CorrectAnsPercentage { get; set; }
        public int TotalQuestions { get; set; }

        public long Score { get; set; }
    }
}
