using Microsoft.EntityFrameworkCore;
using QuizService.Data;
using QuizService.Interfaces.IRepositories;
using QuizService.Models;

namespace QuizService.Repositories
{
    public class CategoryRepo : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
