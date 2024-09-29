namespace MinimalBankAPI.Bussines.Features.Users.Dtos
{
    public class SetUserRolesDto
    {
        public Guid UserId { get; set; }
        public List<Guid> RoleIds { get; set; }
    }
}
