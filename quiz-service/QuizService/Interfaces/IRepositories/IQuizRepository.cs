using QuizService.DTO.QuizDTO;
using QuizService.Models;

namespace QuizService.Interfaces.IRepositories
{
    public interface IQuizRepository
    {
        public Task<Quiz> CreateAsync(Quiz quiz);
        public Task<bool> AddCategoriesToQuizAsync(long quizId, List<long> categoryIds);
        public Task<Quiz?> GetByIdAsync(long id);

        public Task<List<QuizDTO>> GetAllQuizzesAsync();

        public Task<List<Quiz>> GetByCreatorIdAsync(long id);

        public Task<bool> DeleteAsync(long id);

        public Task<QuizWithQuestionsDTO?> GetQuizWithQuestionsAsync(long quizId);
        public Task<Quiz?> UpdateAsync(long id, UpdateQuizDto dto);
        public Task<QuizQuestionsChoicesDTO?> GetQuizWithQuestionsChoicesAsync(long quizId);
        public Task<QuestionFullDTO?> GetQuestionByPageAsync(long quizId, int page);
        public Task<Attempt> CreateAttemptAsync(Attempt attempt);
        public Task<UserAnswer> SaveUserAnswerAsync(UserAnswer answer, List<long> choiceIds);
        public Task<Attempt?> GetAttemptByIdAsync(long id);
        public Task UpdateAttemptAsync(Attempt attempt);

        public Task<List<Attempt>> GetAttemptsByUserAsync(long userId);

        public Task<List<Attempt>> GetUserAttemptsForQuizAsync(long userId, long quizId);

        public Task<(List<Attempt> Attempts, int Total)> GetLeaderboardAsync(
    string? timeFilter, long? quizId, int page, int pageSize);
    }
}
