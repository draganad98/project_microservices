namespace QuizService.Models
{
    public class UserAnswerChoice
    {
        public long UserAnswerId { get; set; }
        public UserAnswer UserAnswer { get; set; } = null!;
        public long ChoiceId { get; set; }
        public Choice Choice { get; set; } = null!;
    }
}
