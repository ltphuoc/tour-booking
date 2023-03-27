using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Account
    {
        public Account()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public int Role { get; set; }
        public string? Avatar { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
