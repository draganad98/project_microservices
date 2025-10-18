namespace QuizService.DTO.QuizDTO
{
    public class QuizResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
        public long QuestionsCount { get; set; }
    }
}
