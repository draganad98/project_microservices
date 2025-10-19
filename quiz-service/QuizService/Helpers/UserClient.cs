namespace QuizService.Helpers
{
    public class UserClient
    {
        private readonly HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<long, string>> GetUsernamesAsync(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
                return new Dictionary<long, string>();

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5287/api/users/usernames", ids);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Dictionary<long, string>>();
            return result ?? new Dictionary<long, string>();
        }
    }

}
