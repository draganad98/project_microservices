using UserService.DTO.UserDTO;
using UserService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace UserService.Helpers
{
    public class UserAutoMapperProfiles : Profile
    {
        public UserAutoMapperProfiles()
        {

            CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

        }
    }
}
