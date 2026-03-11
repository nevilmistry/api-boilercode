using System.ComponentModel.DataAnnotations;

namespace genricRepository.Contracts.Roles.Requests
{
    public class CreateRoleRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
