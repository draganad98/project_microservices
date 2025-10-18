namespace QuizService.DTO.QuizDTO
{
    public class QuestionResponseDto
    {
        public long Id { get; set; }
        public long QuizId { get; set; }
        public string Type { get; set; } = string.Empty;
        public long Points { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<ChoiceResponseDto> Choices { get; set; } = new List<ChoiceResponseDto>();
    }
}
