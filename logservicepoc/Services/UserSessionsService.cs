using logservicepoc.DTO;
using logservicepoc.Models;

namespace logservicepoc.Services
{
    public class UserSessionsService : IUserSessionsService
    {
        private readonly IDbService _dB;
        public UserSessionsService(IDbService dbService)
        {
            _dB = dbService;
        }

        public async Task<ResponseObject> CreateSessions(UserSessions sessions){
            ResponseObject respObj = new ResponseObject();
            try
            {
                var sessionData = await _dB.InsertData("INSERT INTO usersessions (user_id, token, ip_address, user_agent, is_valid, created_at, expires_at) VALUES (@UserId, @Token, @IpAddress, @UserAgent, @IsValid, @CreatedAt, @ExpiresAt) RETURNING session_id", sessions);
                respObj.Error = false;
                respObj.Data = sessionData;
            }
            catch (Exception e)
            {
                //string[] errorMessage = e.Message.Split(":");
                respObj.Message = e.Message;
                respObj.Error = true;
            }
            return respObj;
        }
    }
}