namespace QuizService.DTO.QuizDTO
{
    public class CreateAttemptDTO
    {
        public long QuizId { get; set; }
        public long PlayerId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
    }
}
