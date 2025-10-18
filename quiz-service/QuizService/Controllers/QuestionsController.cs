using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizService.DTO.QuizDTO;
using QuizService.Interfaces.IServices;

namespace QuizService.Controllers
{
    [ApiController]
    [Route("api/quiz/{quizId}/[controller]")]

    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateQuestion(long quizId, [FromBody] CreateQuestionDto createQuestionDto)
        {
            var result = await _questionService.CreateQuestionAsync(createQuestionDto, quizId);

            if (result == null)
                return BadRequest("Failed to create question");

            return CreatedAtAction(nameof(GetQuestion), new { quizId, id = result.Id }, result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestion(long quizId, long id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);

            if (question == null || question.QuizId != quizId)
                return NotFound();

            return Ok(question);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestionsByQuiz(long quizId)
        {
            var questions = await _questionService.GetQuestionsByQuizIdAsync(quizId);
            return Ok(questions);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin")]
        public async Task<IActionResult> DeleteQuestion(long quizId, long id)
        {
            try
            {
                var success = await _questionService.DeleteQuestionAsync(id);
                if (!success)
                    return NotFound($"Question with id {id} not found.");

                return Ok($"Question with id {id} successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteQuestion: {ex.Message}");
                return StatusCode(500, "Internal server error while deleting question.");
            }
        }

    }
}
