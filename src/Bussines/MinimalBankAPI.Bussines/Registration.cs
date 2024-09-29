//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using MinimalBankAPI.Bussines.Base.Rules;
//using MinimalBankAPI.Bussines.Features.Auth.Services;
//using MinimalBankAPI.DataAccess.Context;
//using MinimalBankAPI.DataAccess.Repositories.Abstract;
//using MinimalBankAPI.DataAccess.Repositories.Abstract.Base;
//using MinimalBankAPI.DataAccess.Repositories.Concrete;
//using MinimalBankAPI.DataAccess.Repositories.Concrete.Base;
//using MinimalBankAPI.DataAccess.UnitOfWorks;
//using System.Reflection;

//namespace MinimalBankAPI.Bussines
//{
//    public static class Registration
//    {
//        public static IServiceCollection AddDataLayerDPIs(this IServiceCollection services, IConfiguration configration)
//        {
//            // Add DbContext with connection string from appsettings.json
//            services.AddDbContext<AppDbContext>(options =>
//                options.UseSqlServer(configration.GetConnectionString("DefaultConnection")));

//            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
//            services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
//            services.AddScoped<IRoleOperationClaimRepository, RoleOperationClaimRepository>();
//            services.AddScoped<IRoleRepository, RoleRepository>();
//            services.AddScoped<IUserRepository, UserRepository>();
//            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
//            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

//            return services;
//        }

//        public static IServiceCollection AddBussinesLayer(this IServiceCollection services)
//        {
//            var assembly = Assembly.GetExecutingAssembly();
//            services.AddAutoMapper(assembly);

//            services.AddRulesFromAssemblyContaining(assembly, typeof(BaseRules));

//            services.AddHttpContextAccessor();
//            services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

//            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

//            services.AddScoped<IAuthService, AuthService>();
//            //services.AddScoped<IOperationClaimService, OperationClaimService>();
//            //services.AddScoped<IRoleService, RoleService>();
//            //services.AddScoped<IUserService, UserService>();

//            return services;
//        }

//        private static IServiceCollection AddRulesFromAssemblyContaining(this IServiceCollection services,
//                  Assembly assembly,
//                  Type type)
//        {
//            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
//            foreach (var t in types)
//            {
//                services.AddTransient(t);
//            }
//            return services;
//        }
//    }
//}
