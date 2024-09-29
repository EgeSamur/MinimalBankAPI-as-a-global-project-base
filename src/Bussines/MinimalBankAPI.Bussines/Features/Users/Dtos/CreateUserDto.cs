namespace MinimalBankAPI.Bussines.Features.Users.Dtos
{
    public class CreateUserDto
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
    }
}
