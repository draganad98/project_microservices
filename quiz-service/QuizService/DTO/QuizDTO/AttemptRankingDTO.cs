namespace QuizService.DTO.QuizDTO
{
    public class AttemptRankingDTO
    {
        public int Position { get; set; }
        public string Username { get; set; } = string.Empty;
        public long Score { get; set; }
        public DateTime FinishedAt { get; set; }
        public string Picture { get; set; } = string.Empty;
    }

    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
