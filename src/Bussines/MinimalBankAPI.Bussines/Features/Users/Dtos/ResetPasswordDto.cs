namespace MinimalBankAPI.Bussines.Features.Users.Dtos
{
    public class ResetPasswordDto
    {
        public string EmailAdress { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
