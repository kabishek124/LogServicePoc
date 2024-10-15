using logservicepoc.DTO;
namespace logservicepoc.Utils
{
    public class CommonFunction
    {
        public HashedPasswordResponse hashedPassword(string password){
            var salt = BCrypt.Net.BCrypt.GenerateSalt(12);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            var hashObj = new HashedPasswordResponse{
                hashedPassword = hashedPassword,
                Salt = salt
            };

            return hashObj;
        } 
    }
}