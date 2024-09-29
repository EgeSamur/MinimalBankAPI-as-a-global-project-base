namespace MinimalBankAPI.Bussines.Features.OperationClaims.Dtos
{
    public class UpdateOperationClaimDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
