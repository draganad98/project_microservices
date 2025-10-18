using QuizService.Interfaces.IRepositories;
using QuizService.Models;
using QuizService.Data;
using Microsoft.EntityFrameworkCore;

namespace QuizService.Repositories
{
    public class QuestionRepo : IQuestionRepository
    {
        private readonly DataContext _context;

        public QuestionRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Question> CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> AddChoicesToQuestionAsync(long questionId, List<Choice> choices)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null) return false;

            foreach (var choice in choices)
            {
                choice.QuestionId = questionId;
                _context.Choices.Add(choice);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Question?> GetByIdAsync(long id)
        {
            return await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Question>> GetByQuizIdAsync(long quizId)
        {
            return await _context.Questions
                .Include(q => q.Choices)
                .Where(q => q.QuizId == quizId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(long questionId)
        {

            var question = await _context.Questions
                .Include(q => q.Choices)
                    .ThenInclude(c => c.UserAnswerChoices)
                .Include(q => q.UserAnswers)
                    .ThenInclude(ua => ua.UserAnswerChoices)
                .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
                return false;

            var quizId = question.QuizId;


            foreach (var userAnswer in question.UserAnswers)
            {
                if (userAnswer.UserAnswerChoices.Any())
                    _context.UserAnswerChoices.RemoveRange(userAnswer.UserAnswerChoices);
            }


            if (question.UserAnswers.Any())
                _context.UserAnswers.RemoveRange(question.UserAnswers);


            foreach (var choice in question.Choices)
            {
                if (choice.UserAnswerChoices.Any())
                    _context.UserAnswerChoices.RemoveRange(choice.UserAnswerChoices);
            }


            if (question.Choices.Any())
                _context.Choices.RemoveRange(question.Choices);


            _context.Questions.Remove(question);


            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId);
            if (quiz != null)
            {

                quiz.QuestionsNum = Math.Max(quiz.QuestionsNum - 1, 0);
                _context.Quizzes.Update(quiz);
            }


            await _context.SaveChangesAsync();

            return true;
        }




    }
}
