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
        public string? City { get; set; } = null!;
        public string? Province { get; set; } = null!;
        public string? District { get; set; } = null!;
        public string? Avatar { get; set; } = null!;
        public int Role { get; set; } = 0!;
        public int Status { get; set; } = 0!;
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
