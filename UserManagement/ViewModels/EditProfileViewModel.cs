using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels
{
    public class EditProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
    }
}
