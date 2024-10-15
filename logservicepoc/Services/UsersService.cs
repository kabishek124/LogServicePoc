using logservicepoc.Utils;
using logservicepoc.Models;
using logservicepoc.Services;
using logservicepoc.DTO;

namespace logservicepoc.Services
{
    public class UsersService : IUsersService
    {
        private readonly IDbService _dB;

        public UsersService(IDbService dBService)
        {
            _dB = dBService;
        }

        public async Task<Users> CreateUser(UserReq user){
            try
            {
                var commFunc = new CommonFunction();
                HashedPasswordResponse hashPassword = commFunc.hashedPassword(user.Password);

                var newUser = new Users{
                    Email = user.Email,
                    PasswordHash = hashPassword.hashedPassword,
                    Salt = hashPassword.Salt,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleId = 1,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                string sql = @"
                    INSERT INTO users 
                    (email, password_hash, salt, first_name, last_name, role_id, is_active, last_login_at, created_at, updated_at)
                    VALUES 
                    (@Email, @PasswordHash, @Salt, @FirstName, @LastName, @RoleId, @IsActive, @LastLoginAt, @CreatedAt, @UpdatedAt)
                    RETURNING user_id
                ";

                int userId = await _dB.InsertData(sql, newUser);
                newUser.UserId = userId;
                return newUser;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        // public async Task<User> UpdateUser(User user){

        // }

        // public async Task<bool> DeleteUser(int id){

        // }

        // public async Task<User> GetUser(int id){

        // }

        public async Task<dynamic> GetAllUsers(ListReq listReq){

            var respObj = new ResponseObject();            
            try
            {

                string listQuery = "SELECT users.user_id as UserId, users.email as Email, users.first_name as FirstName, users.last_name as LastName, users.role_id as RoleId, users.is_active as IsActive, users.last_login_at as LastLoginAt, users.created_at as CreatedAt, users.updated_at as UpdatedAt, roles.role_id as Role FROM users users LEFT JOIN roles roles ON roles.role_id = users.role_id";
                UserListResponse userObj = new UserListResponse();
                // The reason for the error when removing the @ symbol is that it is used to denote a verbatim string literal in C#. 
                // Without the @ symbol, the string would be interpreted as a regular string and the newline characters would cause a compiler error.

                userObj.Count = 2;
                var userList = await _dB.GetAll<dynamic>(listQuery + " LIMIT " + listReq.Limit + " OFFSET " + listReq.Index, new{ });

                List<UserResponse> listData = new List<UserResponse>();

                foreach (var user in userList){
                    var data = new UserResponse{
                        UserId = user.UserId,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IsActive = user.IsActive,
                        LastLoginAt = user.LastLoginAt,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                    };
                    listData.Add(data);
                }
                userObj.Users = new List<UserResponse>();
                userObj.Users = listData;

                respObj.StatusCode = 200;
                respObj.Error = false;
                respObj.Message = "Success";
                respObj.Data = userObj;

            }
            catch (Exception e)
            {
                string[] errorMessage = e.Message.Split(":");
                respObj.StatusCode = 500;
                respObj.Message = errorMessage[1];
                respObj.Error = true;
            }
            return respObj;
        }
    }
}