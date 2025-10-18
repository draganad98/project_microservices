namespace QuizService.Models
{
    public class Question
    {
        public long Id { get; set; }
        public long QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public string Type { get; set; } = string.Empty;
        public long Points { get; set; }
        public string Text { get; set; } = string.Empty;
        public string CorrectText { get; set; } = string.Empty;

        public bool Correct { get; set; } = false;
        public ICollection<Choice> Choices { get; set; } = new List<Choice>();
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

    }
}
