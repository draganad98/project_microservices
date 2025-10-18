namespace QuizService.Models
{
    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<QuizCategory> QuizCategories { get; set; } = new List<QuizCategory>();
    }
}
