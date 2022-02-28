using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Placeholder.Primary.Foundation;
using Placeholder.Web.Exceptions;
using Unity;
using Zero.Foundation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Placeholder.Common;
using System.Text;
using Placeholder.Web.Security;
using System;
using Placeholder.Primary.Business.Direct;
using System.Collections.Generic;
using Placeholder.Domain;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authorization;

namespace Placeholder.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, UnityContainer container)
        {
            this.Configuration = configuration;
            this.Container = container;
            this.WebHostEnvironment = webHostEnvironment;
        }

        public IUnityContainer Container { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; set; }
        public IConfiguration Configuration { get; }
        public IFoundation IFoundation { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            this.IFoundation = services.AddZeroFoundation(this.Container, new PlaceholderBootStrap(this.Container, this.WebHostEnvironment, services, this.Configuration));

            services.AddCors(options =>
            {
                options.AddPolicy(nameof(Placeholder.Website),
                                    builder =>
                                    {
                                        builder.WithOrigins(this.GetAllowedDomains())
                                            .AllowCredentials()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                    });
            });
            services.AddControllersWithViews(options => options.Filters.Add(new HttpResponseExceptionFilter()))
                    .AddControllersAsServices()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                    .AddRazorRuntimeCompilation();


            string signingKeyString = this.Configuration.GetValue<string>(CommonAssumptions.APP_KEY_JWT_SIGNING_KEY);
            byte[] signingKey = Encoding.ASCII.GetBytes(signingKeyString);

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddScheme<ApiAccountAuthenticationSchemeOptions, ApiAccountAuthenticationHandler>(ApiAccountAuthenticationHandler.SCHEME, null)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services
                .AddAuthorization(options =>
                {
                    AuthorizationPolicyBuilder builder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme,
                                                                                        ApiAccountAuthenticationHandler.SCHEME,
                                                                                        JwtBearerDefaults.AuthenticationScheme);
                    builder = builder.RequireAuthenticatedUser();
                    options.DefaultPolicy = builder.Build();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseZeroFoundation(this.IFoundation);
            
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("en-US");

            app.UseRequestLocalization(localizationOptions);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = app.ApplicationServices.GetService<IFileProvider>(),
            });

            app.UseRouting();

            app.UseCors(nameof(Placeholder.Website));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict,
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        protected string[] GetAllowedDomains()
        {
            List<string> domains = new List<string>();

            IShopBusiness shopBusiness = this.IFoundation.SafeResolve<IShopBusiness>();
            List<Shop> shops = shopBusiness.Find(0, int.MaxValue);
            foreach (Shop shop in shops)
            {
                domains.Add(string.Format("https://{0}", shop.public_domain));
                domains.Add(string.Format("https://{0}", shop.private_domain));
            }
            shops.Select(x => x.public_domain).ToList();

            return domains.ToArray();
        }
    }
}
