using AutoMapper;
using Microsoft.AspNetCore.Http;
using MinimalBankAPI.DataAccess.UnitOfWorks;
using System.Security.Claims;

namespace MinimalBankAPI.Bussines.Base.Rules
{
    public class BaseService
    {
        public readonly IMapper _mapper;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public string? _userId;

        public BaseService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.ToString();
        }
    }
}
