namespace QuizService.DTO.QuizDTO
{
    public class CreateQuestionDto
    {
        public long Id { get; set; } = 0;
        public string Type { get; set; } = string.Empty;
        public long Points { get; set; }
        public string Text { get; set; } = string.Empty;
        public string CorrectText { get; set; } = string.Empty;

        public bool Correct { get; set; } = false;
        public List<CreateChoiceDto> Choices { get; set; } = new List<CreateChoiceDto>();
    }
}
