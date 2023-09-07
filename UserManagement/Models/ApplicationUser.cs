using Microsoft.AspNetCore.Identity;

namespace UserManagement.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;
        public byte[] ProfilePicture { get; set; } = Array.Empty<byte>();
    }
}
