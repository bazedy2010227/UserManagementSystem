namespace UserManagement.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
    }
}
