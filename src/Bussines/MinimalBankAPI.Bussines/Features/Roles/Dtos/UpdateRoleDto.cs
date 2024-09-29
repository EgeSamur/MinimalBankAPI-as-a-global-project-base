namespace MinimalBankAPI.Bussines.Features.Roles.Dtos
{
    public class UpdateRoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
