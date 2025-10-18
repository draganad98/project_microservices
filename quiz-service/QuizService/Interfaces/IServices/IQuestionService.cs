using QuizService.DTO.QuizDTO;

namespace QuizService.Interfaces.IServices
{
    public interface IQuestionService
    {
        public Task<QuestionResponseDto?> CreateQuestionAsync(CreateQuestionDto createQuestionDto, long quizId);
        public Task<QuestionResponseDto?> GetQuestionByIdAsync(long id);
        public Task<List<QuestionResponseDto>> GetQuestionsByQuizIdAsync(long quizId);
        public Task<QuestionResponseDto?> CreateQuestionWithChoicesAsync(CreateQuestionDto questionDto, long quizId);

        public Task<bool> DeleteQuestionAsync(long questionId);
    }
}
