using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTO.Request
{
    public class AccountRequest
    {
    }

    public class AccountUpdateResquest
    {
        public string FirstName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? Province { get; set; } = null!;
        public string? District { get; set; } = null!;
        public string? Avatar { get; set; } = null!;
        public int Role { get; set; } = 0!;
        public int Status { get; set; } = 0!;
    }

    public class AccountCreateRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? Province { get; set; } = null!;
        public string? District { get; set; } = null!;
        public string? Avatar { get; set; } = null!;
    }

    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class LoginAdminRequest
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
    public class ChangePasswordRequest
    {
        public string Password { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public class ExternalAuthRequest
    {
        [Required]
        public string IdToken { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? Province { get; set; } = null!;
        public string? District { get; set; } = null!;
        public string? Avatar { get; set; } = null!;
    }
}