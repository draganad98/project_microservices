using UserService.DTO.UserDTO;

namespace UserService.Interfaces.IServices;


public interface IUsersService
{
    public Task<string> Register(RegisterUserDTO newUser);
    public Task<string> Authenticate(LoginUserDTO loginUser);
    public Task<Dictionary<long, string>> GetUsernamesByIdsAsync(List<long> ids);
}
