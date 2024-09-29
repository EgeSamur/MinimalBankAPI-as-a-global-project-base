using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MinimalBankAPI.Bussines.Base.Rules;
using MinimalBankAPI.Bussines.Features.Users.Dtos;
using MinimalBankAPI.Bussines.Features.Users.Rules;
using MinimalBankAPI.DataAccess.Repositories.Abstract;
using MinimalBankAPI.DataAccess.UnitOfWorks;
using MinimalBankAPI.Domain.Entites.Auth;
using MinimalBankAPI.Security.Hashing;

namespace MinimalBankAPI.Bussines.Features.Users.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserRules _userRules;
        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IRoleRepository roleRepository, UserRules userRules, IUserRoleRepository userRoleRepository) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRules = userRules;
            _userRoleRepository = userRoleRepository;
        }

        public async Task CreateAsync(CreateUserDto dto)
        {
            User? user = await _userRepository.GetAsync(x => x.EmailAddress == dto.EmailAddress);
            await _userRules.EnsureUserIsNotExists(user);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            HashingHelper.CreatePasswordHash(dto.Password, out var passwordHash, out var passwordSalt);
            var entity = _mapper.Map<User>(dto);
            entity.Id= Guid.NewGuid();
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;
            entity.UserRoles = new List<UserRole>();
            entity.CreatedBy = currentUserId;

            // bu kısımda tokenı kullanan user çekilip onun rolü dahilindeki default rol eklenebilir
            // mesela kendisi event düzenleyici rolünde üse bu kullanıcıda event düzenleyici olabilir.


            await _userRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _userRepository.GetAsync(i => i.Id == id, enableTracking: true);
            await _userRules.EnsureIsUserExists(entity);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            entity!.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;
            entity.DeletedBy = currentUserId;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetListAsync()
        {
            int size = 2147483647;
            var index = 1;
            var users = await _userRepository.GetAllByPagingAsync(
                currentPage: index,
                pageSize: size,
                include: i => i
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .ThenInclude(x=>x.RoleOperationClaims) 
                    .ThenInclude(x=>x.OperationClaim),
                predicate: i => 
                   i.IsDeleted == false,
                orderBy: i => i.OrderBy(x => x.CreatedDate)
            );;
            var result = _mapper.Map<List<UserDto>>(users);
            return result;
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userRepository.GetAsync(i => i.EmailAddress == dto.EmailAdress,
            enableTracking: true);
            await _userRules.EnsureIsUserExists(user);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            if (!HashingHelper.VerifyPasswordHash(dto.OldPassword, user!.PasswordHash, user.PasswordSalt))
                throw new Exception("Wrong Password!");
            HashingHelper.CreatePasswordHash(dto.NewPassword, out var passwordHash, out var passwordSalt);

            user!.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UpdatedDate = DateTime.UtcNow;
            user.UpdatedBy = currentUserId;
            await _userRepository.UpdateAsync(user);
        }

        public async Task SetRolesAsync(SetUserRolesDto dto)
        {
            var user = await _userRepository.GetAsync(i => i.Id == dto.UserId,
           include: i => i.Include(x => x.UserRoles)
               .ThenInclude(x => x.Role),
           enableTracking: true);
            await _userRules.EnsureIsUserExists(user);
            user!.UserRoles.Clear();
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            foreach (var roleId in dto.RoleIds)
            {
                var userRole = new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    RoleId = roleId,
                    CreatedBy = currentUserId
                };
                await _userRoleRepository.AddAsync(userRole); // bunu yapmayınca olmayan bir datayı eklediğim için kızıyor.
                user.UserRoles.Add(userRole);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateUserDto dto)
        {
            var entity = await _userRepository.GetAsync(i => i.Id == dto.Id, enableTracking: true);
            await _userRules.EnsureIsUserExists(entity);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            _mapper.Map(dto, entity);  // entity = _mapper.Map<User>(dto)
            // enabletracking açık olduğu için _userRepository.Update(entity) yapmıyoruz.
            entity.UpdatedBy = currentUserId;
            entity.UpdatedDate = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
        }

        Task<IList<UserDto>> IUserService.GetListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
