namespace logservicepoc.DTO
{
    #nullable disable
    public class UserReq
    {
        /// </summary>
        public string Email { get; set; } // Maps to email
        public string Password { get; set; } // Maps to password_hash
        public string FirstName { get; set; } // Maps to first_name
        public string LastName { get; set; } // Maps to last_name
    }

    public class ListReq{
        public int Index { get; set; }
        public int Limit { get; set; }
        public string SearchKey { get; set; }
    }

    public class LoginReq{
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CategoryReq{
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}