namespace PresentationAPI.Models
{
    public class Register
    {        
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Role { get; set; }
        public int? LicenseId { get; set; }
    }
}
