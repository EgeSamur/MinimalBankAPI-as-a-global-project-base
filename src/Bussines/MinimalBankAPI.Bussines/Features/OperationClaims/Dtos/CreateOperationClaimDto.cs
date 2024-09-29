namespace MinimalBankAPI.Bussines.Features.OperationClaims.Dtos
{
    public class CreateOperationClaimDto
    {
        public string Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
