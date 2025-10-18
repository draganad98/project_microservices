namespace QuizService.Models
{
    public class UserAnswer
    {
        public long Id { get; set; }
        public long AttemptId { get; set; }
        public Attempt Attempt { get; set; } = null!;
        public long QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string TextAnswer { get; set; } = string.Empty;

        public bool? Correct { get; set; }
        public bool IsCorrect { get; set; }
        public ICollection<UserAnswerChoice> UserAnswerChoices { get; set; } = new List<UserAnswerChoice>();

    }
}
