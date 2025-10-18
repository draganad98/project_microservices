namespace QuizService.DTO.QuizDTO
{
    public class QuizDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public long TimeLimitSeconds { get; set; }

        public long QuestionsNum { get; set; }

        public List<long> CategoryIds { get; set; } = new();
    }
}
