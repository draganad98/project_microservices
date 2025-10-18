namespace QuizService.DTO.QuizDTO
{
    public class QuizWithQuestionsDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public long TimeLimitSeconds { get; set; }

        public List<long> CategoryIds { get; set; } = new();

        public List<QuestionDTO> Questions { get; set; } = new();
    }

    public class QuestionDTO
    {
        public long Id { get; set; }
        public long QuizId { get; set; }
        public string Type { get; set; } = string.Empty;
        public long Points { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
