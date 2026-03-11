using System.ComponentModel.DataAnnotations;

namespace genricRepository.Contracts.Roles.Requests
{
    public class UpdateRoleRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
