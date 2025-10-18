using AutoMapper;
using QuizService.DTO.QuizDTO;
using QuizService.Interfaces.IRepositories;
using QuizService.Interfaces.IServices;
using QuizService.Models;

namespace QuizService.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IChoiceRepository _choiceRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IChoiceRepository choiceRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _choiceRepository = choiceRepository;
            _mapper = mapper;
        }

        public async Task<QuestionResponseDto?> CreateQuestionWithChoicesAsync(CreateQuestionDto questionDto, long quizId)
        {
            try
            {

                var question = _mapper.Map<Question>(questionDto);
                question.QuizId = quizId;

                var createdQuestion = await _questionRepository.CreateAsync(question);


                if (questionDto.Choices != null && questionDto.Choices.Any())
                {
                    var choices = questionDto.Choices
                        .Select(c => new Choice
                        {
                            Text = c.Text,
                            IsCorrect = c.IsCorrect,
                            QuestionId = createdQuestion.Id
                        }).ToList();

                    await _choiceRepository.CreateRangeAsync(choices);
                }

                return _mapper.Map<QuestionResponseDto>(createdQuestion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateQuestionWithChoicesAsync: {ex.Message}");
                return null;
            }
        }


        public async Task<QuestionResponseDto?> CreateQuestionAsync(CreateQuestionDto createQuestionDto, long quizId)
        {
            try
            {

                var question = _mapper.Map<Question>(createQuestionDto);
                question.QuizId = quizId;


                var createdQuestion = await _questionRepository.CreateAsync(question);


                if (createQuestionDto.Choices.Any())
                {
                    var choices = _mapper.Map<List<Choice>>(createQuestionDto.Choices);
                    foreach (var choice in choices)
                    {
                        choice.QuestionId = createdQuestion.Id;
                        choice.Question = null;
                    }
                    await _choiceRepository.CreateRangeAsync(choices);
                }


                return _mapper.Map<QuestionResponseDto>(createdQuestion);
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public async Task<QuestionResponseDto?> GetQuestionByIdAsync(long id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null) return null;

            return _mapper.Map<QuestionResponseDto>(question);
        }

        public async Task<List<QuestionResponseDto>> GetQuestionsByQuizIdAsync(long quizId)
        {
            var questions = await _questionRepository.GetByQuizIdAsync(quizId);
            return _mapper.Map<List<QuestionResponseDto>>(questions);
        }

        public async Task<bool> DeleteQuestionAsync(long questionId)
        {
            try
            {
                var deleted = await _questionRepository.DeleteAsync(questionId);
                return deleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteQuestionAsync: {ex.Message}");
                return false;
            }
        }

    }
}
