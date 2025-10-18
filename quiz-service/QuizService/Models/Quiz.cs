namespace QuizService.Models
{
    public class Quiz
    {
        public long Id { get; set; }
        public long TimeLimitSeconds { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long QuestionsNum { get; set; }
        public string DifficultyLevel { get; set; } = string.Empty;
        public long CreatorId { get; set; }

        public ICollection<QuizCategory> QuizCategories { get; set; } = new List<QuizCategory>();
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
