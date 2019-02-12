using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nelibur.ObjectMapper;
using NullHab.App.Configuration;
using NullHab.App.Dto;
using NullHab.App.Middleware;
using NullHab.AuthCore.Configuration;
using NullHab.AuthCore.Contracts;
using NullHab.AuthCore.Services;
using NullHab.DAL.Models;
using NullHab.DAL.Providers.Identity;
using System;

namespace NullHab.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
                {
                    //Можно переопределить свой валидатор, если понадобится
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false; // требуются ли не алфавитно-цифровые символы
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true; // требуются ли цифры

                    options.User.RequireUniqueEmail = true;
                })
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    var tokenOptions = new AuthOptions(Configuration);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = tokenOptions.Issuer,
                        // будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // установка потребителя токена
                        ValidAudience = tokenOptions.Audience,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = tokenOptions.GetSymmetricSecurityKey(),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddTransient<IRoleStore<Role>, RoleStore>();

#if DEBUG
            var connectionString = Configuration.GetConnectionString("LocalConnection");
#else
            var connectionString = Configuration.GetConnectionString("ProductionConnection");
#endif
            services.AddTransient(provider => new UserTable(connectionString));
            services.AddTransient(provider => new UserClaimsTable(connectionString));

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenManager, TokenManager>();

            services.AddSingleton(provider => new AuthOptions(Configuration));

            services.AddTransient<JWTManagerMiddleware>();


            services.AddMvc(options =>
                {
                    options.Filters.Add(new ExceptionFilter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            Map();

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMiddleware<JWTManagerMiddleware>();

            app.UseMvc();
        }

        public void Map()
        {
            TinyMapper.Bind<User, UserDto>();
        }
    }
}