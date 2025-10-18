namespace QuizService.Models
{
    public class QuizCategory
    {
        public long QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public long CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
