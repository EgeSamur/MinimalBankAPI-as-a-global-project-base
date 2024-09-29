using MinimalBankAPI.Domain.Entites.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalBankAPI.Bussines.Features.OperationClaims.Dtos
{
    public class OperationClaimsDto
    {
        public string Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

    }
}
