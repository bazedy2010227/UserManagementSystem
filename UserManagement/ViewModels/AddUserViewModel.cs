using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels
{
    public class AddUserViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }=string.Empty;
        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }=string.Empty;
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }=string.Empty;
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }=string.Empty;
  
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }=string.Empty;


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }=string.Empty;
        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
    }
}
