using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MinimalBankAPI.Bussines.Base.Rules;
using MinimalBankAPI.Bussines.Features.Auth.Dtos;
using MinimalBankAPI.Bussines.Features.Auth.Rules;
using MinimalBankAPI.DataAccess.Repositories.Abstract;
using MinimalBankAPI.DataAccess.UnitOfWorks;
using MinimalBankAPI.Domain.Entites.Auth;
using MinimalBankAPI.Security.Hashing;
using MinimalBankAPI.Security.JWT;
using System.Security.Claims;
using AccessToken = MinimalBankAPI.Security.JWT.AccessToken;

namespace MinimalBankAPI.Bussines.Features.Auth.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AuthRules _authRules;
        private readonly ITokenHelper _tokenHelper;
        public AuthService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, AuthRules authRules, ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _userRepository = userRepository;
            _authRules = authRules;
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoggedDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetAsync(x => x.EmailAddress == dto.EmailAddress,
                include: i => i.Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RoleOperationClaims)
                .ThenInclude(x => x.OperationClaim));

            await _authRules.EnsureIsUserExists(user);
            await _authRules.EnsurePasswordMatches(user!, dto.Password);

            var rolePermissions = user.UserRoles.SelectMany(x => x.Role.RoleOperationClaims).Select(x => x.OperationClaim.Name).ToList();
            var allPermissions = rolePermissions.Distinct().ToList();
            var roles = user.UserRoles.Select(x => x.Role.Name).ToList();

            AccessToken createdAccessToken = _tokenHelper.CreateToken(user!);
            var result = new LoggedDto
            {
                Id = user.Id,
                Roles = roles,
                AccessToken = createdAccessToken,
                Permissions = allPermissions,
            };
           
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
        public async Task<Task> RegisterAsync(RegisterDto dto)
        {
            var user = await _userRepository.GetAsync(x => x.EmailAddress == dto.EmailAddress, 
                include:i=>i.Include(x=>x.UserRoles)
                .ThenInclude(x=>x.Role)
                .ThenInclude(x=>x.RoleOperationClaims)
                .ThenInclude(x=>x.OperationClaim));

            await _authRules.EnsureUserIsNotExists(user);

            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            HashingHelper.CreatePasswordHash(dto.Password, out var passwordHash, out var passwordSalt);
            var entity = _mapper.Map<User>(dto);
            entity.Id = Guid.NewGuid();
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;
            entity.UserRoles = new List<UserRole>();
            entity.CreatedBy = currentUserId;

            var rolePermissions = user.UserRoles.SelectMany(x => x.Role.RoleOperationClaims).Select(x => x.OperationClaim.Name).ToList();
            var allPermissions = rolePermissions.Distinct().ToList();
            var roles = user.UserRoles.Select(x => x.Role.Name).ToList();

            AccessToken createdAccessToken = _tokenHelper.CreateToken(user!);
            var result = new LoggedDto
            {
                Id = user.Id,
                Roles = roles,
                AccessToken = createdAccessToken,
                Permissions = allPermissions,
            };

            await _userRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Task.CompletedTask;
        }
        public async Task<LoggedDto> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            var principal = _tokenHelper.GetPrincipalFromExpiredToken(refreshToken.AccessToken);
            string? email = principal.FindFirst(ClaimTypes.Email).Value;

            User? user = await _userRepository.GetAsync(x => x.EmailAddress == email,
               include: i => i.Include(x => x.UserRoles)
               .ThenInclude(x => x.Role)
               .ThenInclude(x => x.RoleOperationClaims)
               .ThenInclude(x => x.OperationClaim)
               .Include(x => x.RefreshToken),
               enableTracking:true
               );
               
                
            await _authRules.EnsureIsUserExists(user);
            await _authRules.EnsureUserNotLogOut(user.RefreshToken.RefreshTokenExpirationTime);


            var rolePermissions = user.UserRoles.SelectMany(x => x.Role.RoleOperationClaims).Select(x => x.OperationClaim.Name).ToList();
            var allPermissions = rolePermissions.Distinct().ToList();
            var roles = user.UserRoles.Select(x => x.Role.Name).ToList();

            AccessToken createdAccessToken = _tokenHelper.CreateToken(user!);
            var result = new LoggedDto
            {
                Id = user.Id,
                Roles = roles,
                AccessToken = createdAccessToken,
                Permissions = allPermissions,
            };

            user.RefreshToken.Token = createdAccessToken.RefreshToken;
            user.RefreshToken.RefreshTokenExpirationTime = createdAccessToken.RefreshTokenExpiration;
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
        public async Task RevokeAsync(RevokeDto dto)
        {
            var user = await _userRepository.GetAsync(x => x.EmailAddress == dto.Email,
                include: i => i.Include(x => x.RefreshToken),
                enableTracking:true);
            await _authRules.EnsureIsUserExists(user);

            user.RefreshToken = null;
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task RevokeAllAsync()
        {
            IList<User> users = await _userRepository.GetAllAsync(include:i=>i.Include(x=>x.RefreshToken),enableTracking:true);
            foreach (var user in users)
            {
                user.RefreshToken = null;
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
