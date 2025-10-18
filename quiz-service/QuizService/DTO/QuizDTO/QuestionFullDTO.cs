namespace QuizService.DTO.QuizDTO
{
    public class QuestionFullDTO
    {
        public long Id { get; set; }
        public long QuizId { get; set; }
        public string Type { get; set; } = string.Empty;
        public long Points { get; set; }
        public string Text { get; set; } = string.Empty;

        public string? CorrectText { get; set; }   // samo za fillBlank
        public bool? Correct { get; set; }         // samo za trueFalse
        public List<ChoiceDTO> Choices { get; set; } = new(); // za multiple tipove
    }
}
