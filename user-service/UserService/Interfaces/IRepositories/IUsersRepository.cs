using UserService.Models;

namespace UserService.Interfaces.IRepositories
{
    public interface IUsersRepository
    {
        public Task<bool> Register(User newUser);
        public Task<bool> DoesEmailExist(string email);
        public Task<bool> DoesUsernameExist(string username);
        public Task<User> Authenticate(string email, string password);
    }
}
