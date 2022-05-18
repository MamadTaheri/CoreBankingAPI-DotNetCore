using CoreBanking.API.DAL;
using CoreBanking.API.Services.Implementations;
using CoreBanking.API.Services.Interfaces;
using CoreBanking.API.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBanking.API
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
            services.AddDbContext<CoreBankingDbContext>(q => q.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSwaggerGen(q =>
            {
                q.SwaggerDoc("v2", new OpenApiInfo 
                { 
                    Title = "This is The doc of CoreBanking API Project", 
                    Version = "v2",
                    Description = "Built by mohammad Taheri @2022",
                    Contact = new OpenApiContact
                    {
                        Name = "Mohammad Taheri",
                        Email = "mamad.taheri.68@gmail.com",
                        Url = new Uri("https://github.com/MamadTaheri68")
                    }
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(q =>
            {
                q.SwaggerEndpoint("/swagger/v2/swagger.json", "This is The doc of CoreBanking API Project");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
