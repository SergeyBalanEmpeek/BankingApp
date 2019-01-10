using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BankingApp.DataAccess;
using BankingApp.Services;

namespace BankingApp.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Configure database
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BankingContext>(options => options.UseSqlServer(connection));

            //Register Data Access
            services.AddScoped<IUnitOfWork, UnitOfWork>();      //creates for each request

            //Register Data Access Services
            services.AddScoped<AccountService>();
            services.AddScoped<FinanceService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;           //Should be true at production

                        //Token validation setup
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            //validate token creator
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Tokens:Issuer"],

                            //validate token recipient
                            ValidateAudience = true,
                            ValidAudience = Configuration["Tokens:Issuer"],

                            //validate sign key
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),

                            //expiration check
                            ValidateLifetime = true,
                        };
                    });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot/ClientApp/dist";
            }); 
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
                app.UseExceptionHandler("/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "wwwroot/ClientApp"; 

                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "../BankingApp.Web/ClientApp";
                    spa.UseAngularCliServer(npmScript: "start");
                }
            }); 
        }
    }
}
