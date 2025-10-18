using QuizService.Models;

namespace QuizService.Interfaces.IRepositories
{
    public interface IChoiceRepository
    {
        public Task<Choice> CreateAsync(Choice choice);
        public Task<List<Choice>> CreateRangeAsync(List<Choice> choices);
    }
}
