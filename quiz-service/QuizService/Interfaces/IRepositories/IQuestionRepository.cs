using QuizService.Models;

namespace QuizService.Interfaces.IRepositories
{
    public interface IQuestionRepository
    {
        Task<Question> CreateAsync(Question question);
        Task<bool> AddChoicesToQuestionAsync(long questionId, List<Choice> choices);
        Task<Question?> GetByIdAsync(long id);
        Task<List<Question>> GetByQuizIdAsync(long quizId);
        Task<bool> DeleteAsync(long questionId);


    }
}
