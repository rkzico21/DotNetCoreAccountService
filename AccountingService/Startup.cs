using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingService.Authentication;
using AccountingService.DbContexts;
using AccountingService.Entities;
using AccountingService.Filetes;
using AccountingService.Helpers;
using AccountingService.Middlewares;
using AccountingService.Models;
using AccountingService.Repositories;
using AccountingService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountingService
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
            services.AddMvc().
                    AddJsonOptions(options=> 
                    {
                        options.SerializerSettings.DateFormatString ="yyyy-MM-dd";
                        options.SerializerSettings.Converters.Add(new TransactionConverter());
                    }).
                    SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                    
                });

            
            //TODO: create a new class to represent user.
            services.AddScoped<PasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<PasswordHasher<SignUpModel>, PasswordHasher<SignUpModel>>();

            /*services.AddDbContext<AccountingDbContext>(opt => 
            {
                opt.UseInMemoryDatabase("AccountingDb");
                opt.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
               
            });*/
            services.AddDbContext<AccountingDbContext>(opt => 
                    opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))); 
            
            services.AddScoped<AccountRepository, AccountRepository>();
            services.AddScoped<AccountGroupRepository, AccountGroupRepository>();
            services.AddScoped<AccountTypeRepository, AccountTypeRepository>();
            services.AddScoped<TransactionRepository, TransactionRepository >();
            services.AddScoped<OrganizationRepository, OrganizationRepository>();
            services.AddScoped<UserRepository, UserRepository>();
           
            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<TransactionService, TransactionService>();
            services.AddScoped<OrganizationService, OrganizationService>();
            services.AddScoped<AuthenticationService, AuthenticationService>();
            services.AddScoped<UserService, UserService>();
            services.AddScoped<SignUpService, SignUpService>();

            
            
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<Amazon.S3.IAmazonS3>();
            services.AddCors();

             


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(
                options => {
                    options.AddPolicy("AccountAccessPolicy", policy =>
                    policy.Requirements.Add(new AccountAccessRequirement()));

                    options.AddPolicy("TransactionAccessPolicy", policy =>
                    policy.Requirements.Add(new TransactionAccessRequirement()));
                }   
            );

            services.AddScoped<IAuthorizationHandler, AccountAccessHandler>();
            services.AddScoped<IAuthorizationHandler, TransactionAccessHandler>();

           

           /* services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseCors(opt =>opt.WithHeaders("*").WithMethods("*").WithOrigins("*"));

            // required for in memory
            //this.SeedData(app);

            app.UseMiddleware(typeof(ExceptionHandler));
            app.UseAuthentication();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }

        
        private void SeedData(IApplicationBuilder app)
        {
             using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<AccountingDbContext>();
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
        }



    }
}
