using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Formacion.Azure.EntraID.WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

            builder.Services.AddAuthorization(polices => {
                polices.AddPolicy("Rol-Admin", 
                    c => c.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Administradores"));
                polices.AddPolicy("Rol-User", 
                    c => c.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Usuarios"));
                polices.AddPolicy("Super-Admin", 
                    c => c.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", "XLab-MZC-061@xtremelabs.us"));
                polices.AddPolicy("Admin-Seniors", 
                    c => c.RequireClaim("groups", "4641cfc4-111d-4c35-94a4-b58729f6d0d2"));
            });

            builder.Services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.Services.AddRazorPages().AddMicrosoftIdentityUI();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
