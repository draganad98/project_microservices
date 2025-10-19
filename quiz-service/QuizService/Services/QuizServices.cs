using AutoMapper;
using QuizService.Data;
using QuizService.DTO.QuizDTO;
using QuizService.Helpers;
using QuizService.Interfaces.IRepositories;
using QuizService.Interfaces.IServices;
using QuizService.Models;

namespace QuizService.Services
{
    public class QuizServices : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionService _questionService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly UserClient _userClient;

        public QuizServices(IQuizRepository quizRepository,
                          IQuestionService questionService,
                          IMapper mapper,
                          DataContext context,
                          ICategoryRepository categoryRepository,
                          UserClient userClient)
        {
            _quizRepository = quizRepository;
            _questionService = questionService;
            _mapper = mapper;
            _context = context;
            _categoryRepository = categoryRepository;
            _userClient = userClient;
        }

        public async Task<QuizResponseDto?> CreateQuizAsync(CreateQuizDto createQuizDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var quiz = _mapper.Map<Quiz>(createQuizDto);

                quiz.QuestionsNum = createQuizDto.Questions.Count;

                var createdQuiz = await _quizRepository.CreateAsync(quiz);


                if (createQuizDto.CategoryIds.Any())
                {
                    await _quizRepository.AddCategoriesToQuizAsync(createdQuiz.Id, createQuizDto.CategoryIds);
                }


                foreach (var questionDto in createQuizDto.Questions)
                {

                    await _questionService.CreateQuestionWithChoicesAsync(questionDto, createdQuiz.Id);
                }

                await transaction.CommitAsync();

                return _mapper.Map<QuizResponseDto>(createdQuiz);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error in CreateQuizAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<QuizResponseDto?> GetQuizByIdAsync(long id)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            if (quiz == null) return null;

            return _mapper.Map<QuizResponseDto>(quiz);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategories();
        }

        public async Task<List<QuizDTO>> GetAllQuizzesAsync()
        {
            return await _quizRepository.GetAllQuizzesAsync();
        }

        public async Task<List<QuizResponseDto>> GetQuizzesByCreatorIdAsync(long id)
        {
            var quizzes = await _quizRepository.GetByCreatorIdAsync(id);
            return _mapper.Map<List<QuizResponseDto>>(quizzes);
        }

        public async Task<bool> DeleteQuizAsync(long quizId)
        {
            try
            {
                var deleted = await _quizRepository.DeleteAsync(quizId);
                return deleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteQuizAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<QuizWithQuestionsDTO?> GetQuizWithQuestionsAsync(long quizId)
        {
            return await _quizRepository.GetQuizWithQuestionsAsync(quizId);
        }

        public async Task<QuizQuestionsChoicesDTO?> GetQuizWithQuestionsChoicesAsync(long quizId)
        {
            return await _quizRepository.GetQuizWithQuestionsChoicesAsync(quizId);
        }
        public async Task<Quiz?> UpdateQuizAsync(long quizId, UpdateQuizDto dto)
        {
            return await _quizRepository.UpdateAsync(quizId, dto);
        }

        public async Task<QuestionFullDTO?> GetQuestionByPageAsync(long quizId, int page)
        {
            return await _quizRepository.GetQuestionByPageAsync(quizId, page);
        }

        public async Task<AttemptResponseDTO> CreateAttemptAsync(CreateAttemptDTO dto)
        {
            var attempt = new Attempt
            {
                QuizId = dto.QuizId,
                PlayerId = dto.PlayerId,
                StartedAt = dto.StartedAt,
                FinishedAt = dto.FinishedAt
            };

            var created = await _quizRepository.CreateAttemptAsync(attempt);

            return new AttemptResponseDTO
            {
                Id = created.Id,
                QuizId = created.QuizId,
                PlayerId = created.PlayerId,
                StartedAt = created.StartedAt,
                FinishedAt = created.FinishedAt
            };
        }

        public async Task SaveUserAnswerAsync(CreateUserAnswerDTO dto)
        {
            var answer = new UserAnswer
            {
                AttemptId = dto.AttemptId,
                QuestionId = dto.QuestionId,
                TextAnswer = dto.TextAnswer ?? string.Empty,
                Correct = dto.Correct
            };

            await _quizRepository.SaveUserAnswerAsync(answer, dto.ChoiceIds);
        }


        public async Task<AttemptResponseDTO> FinishAttemptAsync(long attemptId)
        {
            var attempt = await _quizRepository.GetAttemptByIdAsync(attemptId);
            if (attempt == null)
                throw new Exception("Attempt not found");

            var userAnswers = attempt.UserAnswers;
            long correctCount = 0;
            long totalScore = 0;


            var totalQuestions = attempt.Quiz.Questions.Count;

            foreach (var ans in userAnswers)
            {
                bool isCorrect = false;

                switch (ans.Question.Type)
                {
                    case "fillBlank":
                        isCorrect = string.Equals(
                            ans.TextAnswer?.Trim(),
                            ans.Question.CorrectText?.Trim(),
                            StringComparison.OrdinalIgnoreCase
                        );
                        break;

                    case "trueFalse":
                        isCorrect = ans.Correct == ans.Question.Correct;
                        break;

                    case "multipleOne":
                    case "multipleMore":
                        var correctChoiceIds = ans.Question.Choices
                            .Where(c => c.IsCorrect)
                            .Select(c => c.Id)
                            .ToList();

                        var userChoiceIds = ans.UserAnswerChoices
                            .Select(c => c.ChoiceId)
                            .ToList();

                        isCorrect = correctChoiceIds.Count == userChoiceIds.Count &&
                                    !correctChoiceIds.Except(userChoiceIds).Any();
                        break;
                }

                ans.IsCorrect = isCorrect;

                if (isCorrect)
                {
                    correctCount++;
                    totalScore += ans.Question.Points;
                }
            }

            attempt.CorrectAnsNum = correctCount;


            attempt.CorrectAnsPercentage = totalQuestions > 0
                ? (double)correctCount / totalQuestions * 100
                : 0;

            attempt.Score = totalScore;
            attempt.FinishedAt = DateTime.UtcNow;
            attempt.DurationSeconds = (int)(attempt.FinishedAt - attempt.StartedAt).TotalSeconds;
            await _quizRepository.UpdateAttemptAsync(attempt);

            return new AttemptResponseDTO
            {
                Id = attempt.Id,
                QuizId = attempt.QuizId,
                PlayerId = attempt.PlayerId,
                StartedAt = attempt.StartedAt,
                FinishedAt = attempt.FinishedAt,
                CorrectAnsNum = attempt.CorrectAnsNum,
                CorrectAnsPercentage = attempt.CorrectAnsPercentage,
                Score = attempt.Score,
                TotalQuestions = totalQuestions,
                DurationSeconds = attempt.DurationSeconds
            };
        }

        public async Task<object> GetAttemptAnswersAsync(long attemptId, int page, int pageSize)
        {
            var attempt = await _quizRepository.GetAttemptByIdAsync(attemptId);
            if (attempt == null) throw new Exception("Attempt not found");

            var answers = attempt.UserAnswers
                .Select(a => new
                {
                    QuestionText = a.Question.Text,
                    QuestionType = a.Question.Type,
                    Choices = a.Question.Choices.Select(c => new { c.Id, c.Text, c.IsCorrect }).ToList(),
                    UserChoiceIds = a.UserAnswerChoices.Select(uac => uac.ChoiceId).ToList(),
                    UserTextAnswer = a.TextAnswer,
                    CorrectTextAnswer = a.Question.CorrectText,
                    IsCorrect = a.IsCorrect,
                    Correct = a.Correct,
                    CorrectTrueFalse = a.Question.Correct,
                    QuizId = a.Attempt.QuizId

                })
                .ToList();

            var total = answers.Count;
            var data = answers.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var quizId = attempt.QuizId;
            return new
            {
                data,
                total,
                page,
                pageSize,
                quizId
            };
        }

        public async Task<(List<Attempt> Attempts, int Total)> GetUserAttemptsAsync(long userId, int page, int pageSize)
        {
            var allAttempts = await _quizRepository.GetAttemptsByUserAsync(userId);
            var total = allAttempts.Count;

            var pagedAttempts = allAttempts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (pagedAttempts, total);
        }

        public async Task<List<Attempt>> GetUserAttemptsForQuizAsync(long userId, long quizId)
        {
            return await _quizRepository.GetUserAttemptsForQuizAsync(userId, quizId);
        }

        public async Task<PagedResult<AttemptRankingDTO>> GetLeaderboardAsync(string? timeFilter, long? quizId, int page, int pageSize)
        {
            var result = await _quizRepository.GetLeaderboardAsync(timeFilter, quizId, page, pageSize);

            
            var playerIds = result.Data.Select(r => long.Parse(r.Username)).Distinct().ToList();
            if (playerIds.Count > 0)
            {
                var users = await _userClient.GetUsersByIdsAsync(playerIds);
                foreach (var item in result.Data)
                {
                    var id = long.Parse(item.Username);
                    if (users.ContainsKey(id))
                    {
                        item.Username = users[id].Username;
                        item.Picture = users[id].Picture;
                    }
                }
            }

            return result;
        }
    }
}
