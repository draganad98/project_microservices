namespace QuizService.DTO.QuizDTO
{
    public class QuizQuestionsChoicesDTO
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public long TimeLimitSeconds { get; set; }
        public List<long> CategoryIds { get; set; } = new();
        public List<QuestionFullDTO> Questions { get; set; } = new();
    }
}
