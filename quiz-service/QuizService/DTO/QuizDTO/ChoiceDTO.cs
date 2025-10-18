namespace QuizService.DTO.QuizDTO
{
    public class ChoiceDTO
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
