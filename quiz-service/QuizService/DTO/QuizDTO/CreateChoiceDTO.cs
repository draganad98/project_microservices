namespace QuizService.DTO.QuizDTO
{
    public class CreateChoiceDto
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
