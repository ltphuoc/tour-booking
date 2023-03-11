using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    public partial class Account
    {
        public Account()
        {
            Bookings = new HashSet<Booking>();
        }

        
        public int Id { get; set; }

        [EmailAddress,Required]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
