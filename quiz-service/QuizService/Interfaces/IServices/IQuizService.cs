using QuizService.DTO.QuizDTO;
using QuizService.Models;

namespace QuizService.Interfaces.IServices
{
    public interface IQuizService
    {
        public Task<QuizResponseDto?> CreateQuizAsync(CreateQuizDto createQuizDto);
        public Task<QuizResponseDto?> GetQuizByIdAsync(long id);

        public Task<List<Category>> GetAllCategoriesAsync();

        public Task<List<QuizDTO>> GetAllQuizzesAsync();

        public Task<List<QuizResponseDto>> GetQuizzesByCreatorIdAsync(long id);

        public Task<bool> DeleteQuizAsync(long id);

        public Task<QuizWithQuestionsDTO?> GetQuizWithQuestionsAsync(long quizId);

        public Task<Quiz?> UpdateQuizAsync(long id, UpdateQuizDto dto);

        public Task<QuizQuestionsChoicesDTO?> GetQuizWithQuestionsChoicesAsync(long quizId);

        public Task<QuestionFullDTO?> GetQuestionByPageAsync(long quizId, int page);
        public Task<AttemptResponseDTO> CreateAttemptAsync(CreateAttemptDTO dto);
        public Task SaveUserAnswerAsync(CreateUserAnswerDTO dto);
        public Task<AttemptResponseDTO> FinishAttemptAsync(long attemptId);
        public Task<object> GetAttemptAnswersAsync(long attemptId, int page, int pageSize);
        public Task<(List<Attempt> Attempts, int Total)> GetUserAttemptsAsync(long userId, int page, int pageSize);
        public Task<List<Attempt>> GetUserAttemptsForQuizAsync(long userId, long quizId);

        Task<PagedResult<AttemptRankingDTO>> GetLeaderboardAsync(string? timeFilter, long? quizId, int page, int pageSize);
    }
}
