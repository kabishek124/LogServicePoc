namespace logservicepoc.Models
{
    public class Users
    {
        public int UserId { get; set; } // Maps to user_id
        public string Email { get; set; } // Maps to email
        public string PasswordHash { get; set; } // Maps to password_hash
        public string Salt { get; set; } // Maps to salt
        public string FirstName { get; set; } // Maps to first_name
        public string LastName { get; set; } // Maps to last_name
        public int RoleId { get; set; } // Maps to role_id (foreign key)
        public bool IsActive { get; set; } // Maps to is_active
        public DateTime? LastLoginAt { get; set; } // Maps to last_login_at (nullable)
        public DateTime CreatedAt { get; set; } // Maps to created_at
        public DateTime? UpdatedAt { get; set; } 
    }

    
}