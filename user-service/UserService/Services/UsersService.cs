using System.Security.Claims;
using System.Text;
using UserService.DTO.UserDTO;
using UserService.Interfaces.IRepositories;
using UserService.Interfaces.IServices;
using UserService.Models;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
namespace UserService.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UsersService(IConfiguration configuration, IUsersRepository usersRepository, IMapper mapper)
        {
            _configuration = configuration;
            _usersRepo = usersRepository;
            _mapper = mapper;

        }

        public async Task<string> Register(RegisterUserDTO newUser)
        {
            if (await _usersRepo.DoesEmailExist(newUser.Email))
                return "emailexists";

            if (await _usersRepo.DoesUsernameExist(newUser.Username))
                return "usernameexists";

            if (newUser.Password.Length < 7)
                return "weakpass";

            var user = _mapper.Map<User>(newUser);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            user.Role = "User";

            if (newUser.Picture != null && newUser.Picture.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newUser.Picture.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newUser.Picture.CopyToAsync(stream);
                }

                user.Picture = "/uploads/" + fileName;
            }
            else
            {

                user.Picture = "/assets/images/avatar.jpg";
            }

            var response = await _usersRepo.Register(user);

            if (!response)
                return "failed";

            return "successful";
        }


        public async Task<string> Authenticate(LoginUserDTO loginUser)
        {
            var user = await _usersRepo.Authenticate(loginUser.EmailOrUsername, loginUser.Password);

            if (user == null)
                return null;

            var token = CreateJWT(user);
            return token;
        }

        private string CreateJWT(User user)
        {
            var secretKey = _configuration.GetSection("AppSettings:Key").Value!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
                new Claim("role", user.Role.ToString()),

            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
