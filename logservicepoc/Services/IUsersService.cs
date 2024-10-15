using logservicepoc.DTO;
using logservicepoc.Models;

namespace logservicepoc.Services
{
    public interface IUsersService
    {
        //  Task<User> GetUserById(int id);
        Task<dynamic> GetAllUsers(ListReq listReq);
        Task<Users> CreateUser(UserReq user);
        //  Task<User> UpdateUser(User user);
        //  Task<bool> DeleteUser(int id);
    }
}