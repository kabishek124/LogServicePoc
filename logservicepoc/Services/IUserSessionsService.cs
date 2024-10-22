using logservicepoc.DTO;
using logservicepoc.Models;

namespace logservicepoc.Services
{
    public interface IUserSessionsService
    {
        Task<ResponseObject> CreateSessions(UserSessions sessions);
    }
}