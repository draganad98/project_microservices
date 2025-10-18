using AutoMapper;
using QuizService.DTO.QuizDTO;
using QuizService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QuizService.Helpers
{
    public class QuizAutoMapperProfiles : Profile
    {
        public QuizAutoMapperProfiles()
        {
            CreateMap<CreateQuizDto, Quiz>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());
            CreateMap<CreateQuestionDto, Question>()
                .ForMember(dest => dest.Choices, opt => opt.Ignore());
            CreateMap<CreateChoiceDto, Choice>();
            CreateMap<Quiz, QuizResponseDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                    src.QuizCategories.Select(qc => qc.Category.Name).ToList()))
                .ForMember(dest => dest.QuestionsCount, opt => opt.MapFrom(src =>
                    src.Questions.Count));

            CreateMap<Question, QuestionResponseDto>();
            CreateMap<Choice, ChoiceResponseDto>();

            /*CreateMap<Quiz, QuizDetailResponseDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                    src.QuizCategories.Select(qc => qc.Category.Name).ToList()))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src =>
                    $"{src.Creator.FirstName} {src.Creator.LastName}"));*/
        }

    }
}
