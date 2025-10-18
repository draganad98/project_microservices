using UserService.Interfaces.IRepositories;
using UserService.Models;
using UserService.Helpers;
using UserService.Data;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repositories
{
    public class UsersRepo : IUsersRepository
    {
        private readonly DataContext _data;
        private readonly AuthenticationHelper _helper;
        public UsersRepo(DataContext data)
        {
            _data = data;
            _helper = new AuthenticationHelper();
        }

        public async Task<bool> Register(User newUser)
        {
            try
            {
                _data.Users.Add(newUser);
                await _data.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DoesEmailExist(string email)
        {
            return await _data.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> DoesUsernameExist(string username)
        {
            return await _data.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User> Authenticate(string emailOrUsername, string password)
        {
            var user = await _data.Users.FirstOrDefaultAsync(x => x.Email == emailOrUsername || x.Username == emailOrUsername);

            if (user == null || user.Password == null)
                return null!;

            if (!_helper.MatchPasswordHash(password, user.Password))
                return null!;

            return user;
        }

    }
}
