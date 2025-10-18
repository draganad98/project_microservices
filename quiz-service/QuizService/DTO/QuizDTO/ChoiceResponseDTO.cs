namespace QuizService.DTO.QuizDTO
{
    public class ChoiceResponseDto
    {
        public long Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
