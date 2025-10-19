using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizService.DTO.QuizDTO;
using QuizService.Interfaces.IServices;
using QuizService.Models;

namespace QuizService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("create")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateQuizDto createQuizDto)
        {
            try
            {

                var result = await _quizService.CreateQuizAsync(createQuizDto);

                if (result == null)
                    return BadRequest("Failed to create quiz");

                return CreatedAtAction(nameof(GetQuiz), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create quiz: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("categories")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin, User")]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await _quizService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin, User")]
        public async Task<IActionResult> GetQuiz(long id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);

            if (quiz == null)
                return NotFound();

            return Ok(quiz);
        }

        [HttpGet("quizzes")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> GetAllQuizzes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5,
        [FromQuery] List<long>? categoryIds = null,
        [FromQuery] string? difficulty = null,
        [FromQuery] string? keyword = null)
        {
            var allQuizzes = await _quizService.GetAllQuizzesAsync();


            if (categoryIds != null && categoryIds.Count > 0)
                allQuizzes = allQuizzes
                    .Where(q => categoryIds.All(id => q.CategoryIds.Contains(id)))
                    .ToList();


            if (!string.IsNullOrEmpty(difficulty))
                allQuizzes = allQuizzes
                    .Where(q => q.DifficultyLevel.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                    .ToList();


            if (!string.IsNullOrEmpty(keyword))
            {
                var lowerKeyword = keyword.Trim().ToLower();
                allQuizzes = allQuizzes
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.Title) && q.Title.ToLower().Contains(lowerKeyword)) ||
                        (!string.IsNullOrEmpty(q.Description) && q.Description.ToLower().Contains(lowerKeyword))
                    )
                    .ToList();
            }


            var total = allQuizzes.Count;


            var pagedQuizzes = allQuizzes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new { data = pagedQuizzes, total, page, pageSize });
        }



        [HttpGet("admin/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin")]
        public async Task<IActionResult> GetQuizzesByCreatorId(long id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var quizzes = await _quizService.GetQuizzesByCreatorIdAsync(id);

            if (quizzes == null || !quizzes.Any())
                return NotFound();


            var totalQuizzes = quizzes.Count;
            var pagedQuizzes = quizzes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                data = pagedQuizzes,
                total = totalQuizzes,
                page,
                pageSize
            });
        }


        [HttpDelete("admin/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin")]
        public async Task<IActionResult> DeleteQuiz(long id)
        {
            try
            {
                var success = await _quizService.DeleteQuizAsync(id);
                if (!success)
                    return NotFound($"Quiz with id {id} not found.");

                return Ok($"Quiz with id {id} successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteQuiz: {ex.Message}");
                return StatusCode(500, "Internal server error while deleting quiz.");
            }
        }

        [HttpGet("{id}/full")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin, User")]
        public async Task<IActionResult> GetQuizWithQuestions(long id)
        {
            try
            {
                var quiz = await _quizService.GetQuizWithQuestionsAsync(id);
                if (quiz == null)
                    return NotFound($"Quiz with id {id} not found.");

                return Ok(quiz);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetQuizWithQuestions: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("admin/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin")]
        public async Task<IActionResult> UpdateQuiz(long id, [FromBody] UpdateQuizDto dto)
        {
            var updated = await _quizService.UpdateQuizAsync(id, dto);
            if (updated == null)
                return NotFound("Quiz not found");

            return Ok(new { message = "Quiz updated successfully" });
        }

        [HttpGet("{quizId}/details")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> GetQuizWithQuestionsChoices(long quizId)
        {
            var quiz = await _quizService.GetQuizWithQuestionsChoicesAsync(quizId);

            if (quiz == null)
                return NotFound("Quiz not found.");

            return Ok(quiz);
        }

        [HttpGet("{quizId}/question")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> GetQuestionByPage(long quizId, [FromQuery] int page = 1)
        {
            var question = await _quizService.GetQuestionByPageAsync(quizId, page);

            if (question == null)
                return NotFound("No more questions.");

            return Ok(question);
        }

        [HttpPost("attempt")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> CreateAttempt([FromBody] CreateAttemptDTO dto)
        {
            var result = await _quizService.CreateAttemptAsync(dto);
            return Ok(result);
        }

        [HttpPost("answer")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> SaveUserAnswer([FromBody] CreateUserAnswerDTO dto)
        {
            await _quizService.SaveUserAnswerAsync(dto);
            return Ok();
        }

        [HttpPost("finish/{attemptId}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> FinishQuiz(long attemptId)
        {
            var result = await _quizService.FinishAttemptAsync(attemptId);
            return Ok(result);
        }

        [HttpGet("attempt/{attemptId}/answers")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> GetAttemptAnswers(long attemptId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = await _quizService.GetAttemptAnswersAsync(attemptId, page, pageSize);
            return Ok(result);
        }

        [HttpGet("myattempts")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> GetMyAttempts(long userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var (attempts, total) = await _quizService.GetUserAttemptsAsync(userId, page, pageSize);

            var result = attempts.Select(a => new
            {
                a.Id,
                Title = a.Quiz.Title,
                StartedAt = a.StartedAt,
                a.Score,
                Percentage = a.CorrectAnsPercentage,
                DurationSeconds = a.DurationSeconds
            }).ToList();

            return Ok(new { data = result, total, page, pageSize });
        }

        [HttpGet("quiz/{quizId}/user-attempts/{userId}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "User")]
        public async Task<IActionResult> GetUserAttemptsForQuiz(long userId, long quizId)
        {
            var attempts = await _quizService.GetUserAttemptsForQuizAsync(userId, quizId);

            var result = attempts
                .Select(a => new
                {
                    a.Id,
                    a.Score,
                    a.StartedAt
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin, User")]
        public async Task<IActionResult> GetAllQuizzes()
        {
            try
            {
                var quizzes = await _quizService.GetAllQuizzesAsync();
                return Ok(quizzes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error while get quizzes.", error = ex.Message });
            }
        }

        [HttpGet("leaderboard")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Admin, User")]
        public async Task<IActionResult> GetLeaderboard([FromQuery] string? timeFilter, [FromQuery] long? quizId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _quizService.GetLeaderboardAsync(timeFilter, quizId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error loading leaderboard", error = ex.Message });
            }
        }
    }
}
