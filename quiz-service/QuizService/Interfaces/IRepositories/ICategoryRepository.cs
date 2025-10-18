using QuizService.Models;

namespace QuizService.Interfaces.IRepositories
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategories();
    }
}
