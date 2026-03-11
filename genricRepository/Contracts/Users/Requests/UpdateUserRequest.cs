using System.ComponentModel.DataAnnotations;

namespace genricRepository.Contracts.Users.Requests
{
    public class UpdateUserRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public Guid RoleId { get; set; }
    }
}
