namespace genricRepository.Contracts.Users.Responses
{
    public class DeleteUserResponse
    {
        public Guid? Id { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
