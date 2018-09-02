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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            services.AddDbContext<AccountingDbContext>(opt => 
                opt.UseInMemoryDatabase("AccountingDb"));  
            

            services.AddScoped<AccountRepository, AccountRepository>();
            services.AddScoped<AccountGroupRepository, AccountGroupRepository>();
            services.AddScoped<AccountTypeRepository, AccountTypeRepository>();
            services.AddScoped<AccountService, AccountService>();
            

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
