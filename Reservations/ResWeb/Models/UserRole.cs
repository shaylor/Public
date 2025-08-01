using ResWeb.Data;

namespace ResWeb.Models
{
    public class UserRole
    {
        public ApplicationUser User { get; set; }
        public string RoleId { get; set; }
    }
}
