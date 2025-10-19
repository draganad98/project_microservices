using QuizService.DTO.QuizDTO;
using QuizService.Interfaces.IRepositories;
using QuizService.Models;
using QuizService.Data;
using Microsoft.EntityFrameworkCore;

namespace QuizService.Repositories
{
    public class QuizRepo : IQuizRepository
    {
        private readonly DataContext _data;

        public QuizRepo(DataContext data)
        {
            _data = data;
        }

        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            _data.Quizzes.Add(quiz);
            await _data.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool> AddCategoriesToQuizAsync(long quizId, List<long> categoryIds)
        {
            var quiz = await _data.Quizzes.FindAsync(quizId);
            if (quiz == null) return false;

            foreach (var categoryId in categoryIds)
            {
                var category = await _data.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    var quizCategory = new QuizCategory
                    {
                        QuizId = quizId,
                        CategoryId = categoryId
                    };
                    _data.QuizCategories.Add(quizCategory);
                }
            }

            await _data.SaveChangesAsync();
            return true;
        }

        public async Task<Quiz?> GetByIdAsync(long id)
        {
            return await _data.Quizzes
                .Include(q => q.QuizCategories)
                .ThenInclude(qc => qc.Category)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<QuizDTO>> GetAllQuizzesAsync()
        {
            return await _data.Quizzes
                .Include(q => q.QuizCategories)
                .Select(q => new QuizDTO
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    DifficultyLevel = q.DifficultyLevel,
                    TimeLimitSeconds = q.TimeLimitSeconds,
                    QuestionsNum = q.QuestionsNum,
                    CategoryIds = q.QuizCategories.Select(qc => qc.CategoryId).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<Quiz>> GetByCreatorIdAsync(long id)
        {
            return await _data.Quizzes
                .Include(q => q.QuizCategories)
                    .ThenInclude(qc => qc.Category)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .Where(q => q.CreatorId == id)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(long quizId)
        {

            var quiz = await _data.Quizzes
                .Include(q => q.QuizCategories)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .Include(q => q.Attempts)
                    .ThenInclude(a => a.UserAnswers)
                        .ThenInclude(ua => ua.UserAnswerChoices)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null) return false;


            if (quiz.QuizCategories.Any())
                _data.QuizCategories.RemoveRange(quiz.QuizCategories);


            foreach (var attempt in quiz.Attempts)
            {
                foreach (var ua in attempt.UserAnswers)
                {
                    if (ua.UserAnswerChoices.Any())
                        _data.UserAnswerChoices.RemoveRange(ua.UserAnswerChoices);
                }
                if (attempt.UserAnswers.Any())
                    _data.UserAnswers.RemoveRange(attempt.UserAnswers);
            }
            if (quiz.Attempts.Any())
                _data.Attempts.RemoveRange(quiz.Attempts);


            foreach (var question in quiz.Questions)
            {
                if (question.Choices.Any())
                    _data.Choices.RemoveRange(question.Choices);
            }
            if (quiz.Questions.Any())
                _data.Questions.RemoveRange(quiz.Questions);


            _data.Quizzes.Remove(quiz);

            await _data.SaveChangesAsync();
            return true;
        }

        public async Task<QuizWithQuestionsDTO?> GetQuizWithQuestionsAsync(long quizId)
        {
            var quiz = await _data.Quizzes
                .Include(q => q.QuizCategories)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null) return null;

            return new QuizWithQuestionsDTO
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                DifficultyLevel = quiz.DifficultyLevel,
                TimeLimitSeconds = quiz.TimeLimitSeconds,
                CategoryIds = quiz.QuizCategories.Select(qc => qc.CategoryId).ToList(),
                Questions = quiz.Questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    QuizId = q.QuizId,
                    Type = q.Type,
                    Points = q.Points,
                    Text = q.Text
                }).ToList()
            };
        }

        public async Task<QuizQuestionsChoicesDTO?> GetQuizWithQuestionsChoicesAsync(long quizId)
        {
            var quiz = await _data.Quizzes
                .Include(q => q.QuizCategories)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                return null;

            return new QuizQuestionsChoicesDTO
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                DifficultyLevel = quiz.DifficultyLevel,
                TimeLimitSeconds = quiz.TimeLimitSeconds,
                CategoryIds = quiz.QuizCategories.Select(qc => qc.CategoryId).ToList(),

                Questions = quiz.Questions.Select(q => new QuestionFullDTO
                {
                    Id = q.Id,
                    QuizId = q.QuizId,
                    Type = q.Type,
                    Points = q.Points,
                    Text = q.Text,


                    CorrectText = q.Type == "fillBlank" ? q.CorrectText : null,

                    Correct = q.Type == "trueFalse" ? q.Correct : (bool?)null,

                    Choices = (q.Type == "multipleOne" || q.Type == "multipleMore")
                        ? q.Choices.Select(c => new ChoiceDTO
                        {
                            Id = c.Id,
                            QuestionId = c.QuestionId,
                            Text = c.Text,
                            IsCorrect = c.IsCorrect
                        }).ToList()
                        : new List<ChoiceDTO>()
                }).ToList()
            };
        }


        public async Task<Quiz?> UpdateAsync(long quizId, UpdateQuizDto dto)
        {
            var quiz = await _data.Quizzes
                .Include(q => q.QuizCategories)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                return null;


            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            quiz.TimeLimitSeconds = dto.TimeLimitSeconds;
            quiz.DifficultyLevel = dto.DifficultyLevel;


            var existingCategories = quiz.QuizCategories.ToList();
            _data.QuizCategories.RemoveRange(existingCategories);

            foreach (var catId in dto.CategoryIds)
            {
                _data.QuizCategories.Add(new QuizCategory
                {
                    QuizId = quiz.Id,
                    CategoryId = catId
                });
            }


            foreach (var qDto in dto.Questions)
            {
                if (qDto.Id != 0 && quiz.Questions.Any(q => q.Id == qDto.Id))
                    continue;

                var newQuestion = new Question
                {
                    QuizId = quiz.Id,
                    Text = qDto.Text,
                    Type = qDto.Type,
                    Points = qDto.Points,
                    Correct = qDto.Correct,
                    CorrectText = qDto.CorrectText,
                    Choices = qDto.Choices?.Select(c => new Choice
                    {
                        Text = c.Text,
                        IsCorrect = c.IsCorrect
                    }).ToList() ?? new List<Choice>()
                };

                quiz.Questions.Add(newQuestion);
            }
            quiz.QuestionsNum = dto.QuestionsNum;
            await _data.SaveChangesAsync();
            return quiz;
        }

        public async Task<QuestionFullDTO?> GetQuestionByPageAsync(long quizId, int page)
        {
            var quiz = await _data.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null) return null;

            var questions = quiz.Questions.OrderBy(q => q.Id).ToList();
            if (page < 1 || page > questions.Count) return null;

            var q = questions[page - 1];

            return new QuestionFullDTO
            {
                Id = q.Id,
                QuizId = q.QuizId,
                Type = q.Type,
                Points = q.Points,
                Text = q.Text,
                CorrectText = q.Type == "fillBlank" ? q.CorrectText : null,
                Correct = q.Type == "trueFalse" ? q.Correct : (bool?)null,
                Choices = (q.Type == "multipleOne" || q.Type == "multipleMore")
                    ? q.Choices.Select(c => new ChoiceDTO
                    {
                        Id = c.Id,
                        QuestionId = c.QuestionId,
                        Text = c.Text,
                        IsCorrect = c.IsCorrect
                    }).ToList()
                    : new List<ChoiceDTO>()
            };
        }

        public async Task<Attempt> CreateAttemptAsync(Attempt attempt)
        {
            _data.Attempts.Add(attempt);
            await _data.SaveChangesAsync();
            return attempt;
        }

        public async Task<UserAnswer> SaveUserAnswerAsync(UserAnswer answer, List<long> choiceIds)
        {
            _data.UserAnswers.Add(answer);
            await _data.SaveChangesAsync();

            if (choiceIds.Any())
            {
                foreach (var id in choiceIds)
                {
                    _data.UserAnswerChoices.Add(new UserAnswerChoice
                    {
                        UserAnswerId = answer.Id,
                        ChoiceId = id
                    });
                }

                await _data.SaveChangesAsync();
            }

            return answer;
        }

        public async Task<Attempt?> GetAttemptByIdAsync(long id)
        {
            return await _data.Attempts
                .Include(a => a.UserAnswers)
                    .ThenInclude(ua => ua.UserAnswerChoices)
                .Include(a => a.UserAnswers)
                    .ThenInclude(ua => ua.Question)
                        .ThenInclude(q => q.Choices)
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task UpdateAttemptAsync(Attempt attempt)
        {
            _data.Attempts.Update(attempt);
            await _data.SaveChangesAsync();
        }

        public async Task<List<Attempt>> GetAttemptsByUserAsync(long userId)
        {
            return await _data.Attempts
                .Include(a => a.Quiz)
                .Where(a => a.PlayerId == userId)
                .OrderByDescending(a => a.StartedAt)
                .ToListAsync();
        }

        public async Task<List<Attempt>> GetUserAttemptsForQuizAsync(long userId, long quizId)
        {
            return await _data.Attempts
                .Include(a => a.Quiz)
                .Where(a => a.PlayerId == userId && a.QuizId == quizId)
                .OrderByDescending(a => a.StartedAt)
                .Take(5)
                .ToListAsync();
        }

        public async Task<PagedResult<AttemptRankingDTO>> GetLeaderboardAsync(string? timeFilter, long? quizId, int page, int pageSize)
        {
            var query = _data.Attempts.AsQueryable();

            var now = DateTime.UtcNow;
            if (timeFilter == "today") query = query.Where(a => a.FinishedAt.Date == now.Date);
            else if (timeFilter == "thisWeek") query = query.Where(a => a.FinishedAt.Date >= now.Date.AddDays(-(int)now.DayOfWeek));
            else if (timeFilter == "thisMonth") query = query.Where(a => a.FinishedAt.Date >= new DateTime(now.Year, now.Month, 1));

            if (quizId.HasValue && quizId != 0) query = query.Where(a => a.QuizId == quizId.Value);

            var total = await query.CountAsync();
            var attempts = await query.OrderByDescending(a => a.Score).ThenBy(a => a.FinishedAt)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var ranked = attempts.Select((a, index) => new AttemptRankingDTO
            {
                Position = ((page - 1) * pageSize) + index + 1,
                Username = a.PlayerId.ToString(), 
                Picture = string.Empty,
                Score = a.Score,
                FinishedAt = a.FinishedAt
            }).ToList();

            return new PagedResult<AttemptRankingDTO>
            {
                Data = ranked,
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}
