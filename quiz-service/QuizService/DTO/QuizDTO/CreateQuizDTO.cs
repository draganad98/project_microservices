namespace QuizService.DTO.QuizDTO
{
    public class CreateQuizDto
    {
        public long CreatorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long TimeLimitSeconds { get; set; }
        //       public long QuestionsNum { get; set; }
        public string DifficultyLevel { get; set; } = string.Empty;
        public List<long> CategoryIds { get; set; } = new List<long>();
        public List<CreateQuestionDto> Questions { get; set; } = new List<CreateQuestionDto>();
    }
}
