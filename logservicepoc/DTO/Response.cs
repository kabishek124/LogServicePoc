namespace logservicepoc.DTO
{
    
    public class HashedPasswordResponse{
        public string? Salt { get; set; }
        public string? hashedPassword { get; set; }
    }

    public class ResponseObject
    {
        public int StatusCode { get; set; }
        public Boolean Error { get; set; }
        public dynamic? Message { get; set; }
        public dynamic? Data { get; set; }
    
    }

    public class UserResponse{
        public int UserId { get; set; } // Maps to user_id
        public string Email { get; set; } // Maps to email
        public string FirstName { get; set; } // Maps to first_name
        public string LastName { get; set; } // Maps to last_name
        public bool IsActive { get; set; } // Maps to is_active
        public DateTime? LastLoginAt { get; set; } // Maps to last_login_at (nullable)
        public DateTime CreatedAt { get; set; } // Maps to created_at
        public DateTime? UpdatedAt { get; set; } 
    }

    public class UserListResponse{
        public List<UserResponse>? Users { get; set; }
        public int? Count {get; set;}
    }
}