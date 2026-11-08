using ModularCommerce.Enums;

namespace ModularCommerce.Entities
{
    public class User
    {
        public long Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Guest;
        public bool IsActive { get; set; }

        // One user can have many orders
        public ICollection<Order> Orders { get; set; } = [];
    }
}

