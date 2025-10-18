using System.ComponentModel.DataAnnotations;

namespace UserService.DTO.UserDTO
{
    public class LoginUserDTO
    {
        [Required]
        public string EmailOrUsername { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
