using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class AccountRequest
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
        public int Role { get; set; } = 0!;
        public int Status { get; set; } = 0!;
    }

    public class AccountUpdateResquest
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
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class ChangePasswordRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public class ExternalAuthRequest
    {
        public string? IdToken { get; set; }
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