namespace QuizService.Models
{
    public class Choice
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public ICollection<UserAnswerChoice> UserAnswerChoices { get; set; } = new List<UserAnswerChoice>();


    }
}
