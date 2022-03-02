using CreateAndAnswerQuiz.Api.DataAccess.Context;
using CreateAndAnswerQuiz.Api.DataAccess.Entities;
using CreateAndAnswerQuiz.Business.Base;
using CreateAndAnswerQuiz.Business.Implementaion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
            services.AddDbContext<ApiDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("cs")));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            })
               .AddEntityFrameworkStores<ApiDbContext>()
               .AddDefaultTokenProviders();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:Key"])),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = Configuration["Token:Audience"],
                    ValidIssuer = Configuration["Token:Issuer"],
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();
            services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }); 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole",
                     policy => policy.RequireRole("Admin"));
            });
            services.AddRouting(x => x.LowercaseUrls = true);
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CreateAndAnswerQuiz.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CreateAndAnswerQuiz.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CreateRolesAndAdmin(serviceProvider);
        }
        private void CreateRolesAndAdmin(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Member" };
            Task<IdentityResult> roleResult;

            foreach (var roleName in roleNames)
            {
                Task<bool> roleExist = RoleManager.RoleExistsAsync(roleName);
                roleExist.Wait();
                if (!roleExist.Result)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = RoleManager.CreateAsync(new IdentityRole(roleName));
                    roleResult.Wait();
                }
            }
            //Ensure you have these values in your appsettings.json file
            string userPWD = Configuration["AppSettings:UserPassword"];
            Task<User> _user = UserManager.FindByEmailAsync(Configuration["AppSettings:UserEmail"]);
            _user.Wait();
            if (_user.Result == null)
            {
                //Here you could create a super user who will maintain the web app
                var poweruser = new User();
                poweruser.UserName = Configuration["AppSettings:UserEmail"];
                poweruser.Email = Configuration["AppSettings:UserEmail"];
                Task<IdentityResult> createPowerUser = UserManager.CreateAsync(poweruser, userPWD);
                createPowerUser.Wait();
                if (createPowerUser.Result.Succeeded)
                {
                    //here we tie the new user to the role
                    Task<IdentityResult> newUserRole = UserManager.AddToRoleAsync(poweruser, "Admin");
                    newUserRole.Wait();
                }
            }
        }
    }
}
