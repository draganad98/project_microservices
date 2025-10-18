namespace QuizService.DTO.QuizDTO
{
    public class CreateUserAnswerDTO
    {
        public long AttemptId { get; set; }
        public long QuestionId { get; set; }
        public string? TextAnswer { get; set; }
        public bool? Correct { get; set; }
        public List<long> ChoiceIds { get; set; } = new List<long>();
    }
}
