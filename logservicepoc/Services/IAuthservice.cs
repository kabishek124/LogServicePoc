using logservicepoc.DTO;
using logservicepoc.Models;

namespace logservicepoc.Services
{
    public interface IAuthservice
    {
        Task<LoginResponse> UserLogin(LoginReq login);
        public bool VerifyPassword(Users user, string password);
        public Task<string> GenerateToken(Users user);

    }
}