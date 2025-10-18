using QuizService.Data;
using QuizService.Interfaces.IRepositories;
using QuizService.Models;

namespace QuizService.Repositories
{
    public class ChoiceRepo : IChoiceRepository
    {
        private readonly DataContext _context;

        public ChoiceRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Choice> CreateAsync(Choice choice)
        {
            _context.Choices.Add(choice);
            await _context.SaveChangesAsync();
            return choice;
        }

        public async Task<List<Choice>> CreateRangeAsync(List<Choice> choices)
        {
            _context.Choices.AddRange(choices);
            await _context.SaveChangesAsync();
            return choices;
        }
    }
}
