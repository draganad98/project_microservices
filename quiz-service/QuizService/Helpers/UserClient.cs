using static System.Net.WebRequestMethods;
using UserService.DTO.UserDTO;
namespace QuizService.Helpers
{
    public class UserClient
    {
        private readonly HttpClient _http;

        public UserClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<Dictionary<long, UserDTO>> GetUsersByIdsAsync(List<long> ids)
        {
            var response = await _http.PostAsJsonAsync("/users/usernames-with-pictures", ids);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Dictionary<long, UserDTO>>() ?? new();
        }


    }

}
