using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingService.DbContexts;
using AccountingService.Filetes;
using AccountingService.Middlewares;
using AccountingService.Repositories;
using AccountingService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                    }).
                    SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                    
                });

            

            services.AddDbContext<AccountingDbContext>(opt => opt.UseInMemoryDatabase("AccountingDb"));
            /*services.AddDbContext<AccountingDbContext>(opt => 
                    opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))); */
            
            services.AddScoped<AccountRepository, AccountRepository>();
            services.AddScoped<AccountGroupRepository, AccountGroupRepository>();
            services.AddScoped<AccountTypeRepository, AccountTypeRepository>();
            services.AddScoped<TransactionRepository, TransactionRepository >();
            services.AddScoped<OrganizationRepository, OrganizationRepository>();
           
            services.AddScoped<AccountService, AccountService>();
            services.AddScoped<TransactionService, TransactionService>();
            services.AddScoped<OrganizationService, OrganizationService>();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<Amazon.S3.IAmazonS3>();
            services.AddCors();
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
            this.SeedData(app);

            app.UseMiddleware(typeof(ExceptionHandler));
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
