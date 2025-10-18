using System.Security.Cryptography;

namespace UserService.Helpers
{
    public class AuthenticationHelper
    {
        public AuthenticationHelper()
        {

        }

        public bool MatchPasswordHash(string passwordText, string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(passwordText, userPassword);

        }

    }
}
