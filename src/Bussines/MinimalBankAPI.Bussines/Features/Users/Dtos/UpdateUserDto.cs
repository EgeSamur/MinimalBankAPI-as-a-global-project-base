namespace MinimalBankAPI.Bussines.Features.Users.Dtos
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public bool Status { get; set; }
    }
}
