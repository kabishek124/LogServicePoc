using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using logservicepoc.DTO;
using logservicepoc.Models;
using Microsoft.IdentityModel.Tokens;

#nullable disable
namespace logservicepoc.Services
{
    public class AuthService : IAuthservice
    {
        private readonly IDbService _dB;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly IUserSessionsService _sessionsService;

        public AuthService(IDbService db, IUserSessionsService userSessionsService)
        {
            _dB = db;
            _sessionsService = userSessionsService;
            _jwtSecret = Environment.GetEnvironmentVariable("JWTKEY");
            _jwtIssuer = Environment.GetEnvironmentVariable("JWTISSUER");
            _jwtAudience = Environment.GetEnvironmentVariable("JWTAUDIENCE");
        }

        public async Task<LoginResponse> UserLogin(LoginReq loginReq){
            LoginResponse response = new LoginResponse();
            try
            {
                if(string.IsNullOrEmpty(loginReq.Email) || string.IsNullOrEmpty(loginReq.Password)){
                    response.StatusCode = 400;
                    response.Error = true;
                    response.Message = "Email and password are required.";
                }else{
                    var user = await _dB.GetAsync<dynamic>(
                        @"SELECT 
                            users.user_id AS UserId, 
                            users.email AS Email,
                            users.password_hash AS PasswordHash, 
                            users.salt AS Salt, 
                            users.first_name AS FirstName, 
                            users.last_name AS LastName, 
                            users.role_id AS RoleId, 
                            users.is_active AS IsActive, 
                            users.last_login_at AS LastLoginAt, 
                            users.created_at AS CreatedAt, 
                            users.updated_at AS UpdatedAt, 
                            roles.role_id AS RoleId 
                        FROM 
                            users 
                        LEFT JOIN 
                            roles ON roles.role_id = users.role_id 
                        WHERE 
                            users.email = @Email AND users.is_active = true",
                        new { Email = loginReq.Email }
                    );
                    if(user == null || user.role_id == 1){
                        response.StatusCode = 404;
                        response.Error = true;
                        response.Message = "User not found.";
                    }else{
                        Users newUser = new Users{
                            UserId = user.userid,
                            RoleId = user.roleid,
                            Email = user.email,
                            PasswordHash = user.passwordhash,
                            Salt = user.salt,
                        };

                        Console.WriteLine(newUser.UserId + "userId frst");
                        // var data = new UserResponse{
                        //     UserId = user.user_id,
                        //     Email = user.email,
                        //     FirstName = user.first_name,
                        //     LastName = user.last_name,
                        // };

                        if(!VerifyPassword(newUser, loginReq.Password)){
                            response.StatusCode = 401;
                            response.Error = true;
                            response.Message = "Invalid password.";
                        }else{
                            var token = await GenerateToken(newUser);
                            if (token == null){
                                response.StatusCode = 500;
                                response.Error = true;
                                response.Message = "Failed to generate token.";
                            }else{
                                var result = await _dB.EditData("UPDATE usersessions SET is_valid = false WHERE user_id = @UserId", new {UserId = newUser.UserId});
                                UserSessions sessions = new UserSessions{
                                    UserId = newUser.UserId,
                                    Token = token,
                                    CreatedAt = DateTime.Now,
                                    IsValid = true,
                                    ExpiresAt = DateTime.Now.AddMinutes(60),
                                    IpAddress = "string",
                                    UserAgent = "string"
                                };
                                
                                ResponseObject obj = await _sessionsService.CreateSessions(sessions);
                                if(obj.Error == true){
                                    response.StatusCode = 400;
                                    response.Error = true;
                                    response.Message = obj.Message;
                                }else{
                                    response.StatusCode = 200;
                                    response.Error = false;
                                    response.Message = "Login successful.";
                                    response.Token = token;
                                }
                            }
                            //response.Data = data;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                response.Error = true;
                response.StatusCode = 500;
                response.Message = e.Message;
            }
            return response;
        }

        public bool VerifyPassword(Users user, string password){
            if(user == null || password == null){
                return false;
            }

            var salt = user.Salt;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            if(hashedPassword != user.PasswordHash){
                return false;
            }

            return true;
        }

        public async Task<string> GenerateToken(Users user){
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);


                var roles = await _dB.GetAsync<string>("select roles.role_name as role from roles where roles.role_id = @RoleId", new {user.RoleId});
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Role, roles)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60), // Token expires in 15 minutes
                    Issuer = _jwtIssuer,
                    Audience = _jwtAudience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                
                throw new Exception("Failed to generate token: " + e.Message);
            }
        }
    }
}