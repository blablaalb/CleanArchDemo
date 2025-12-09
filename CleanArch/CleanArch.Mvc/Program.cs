using CleanArch.Infra.Data.Context;
using CleanArch.Infra.IoC;
using CleanArch.Mvc.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// This method gets called by the runtime. Use this method to add services to the container.
void ConfigureServices()
{
    var services = builder.Services;
    services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            Configuration.GetConnectionString("UniversityIdentityDBConnection")));
    services.AddDefaultIdentity<IdentityUser>()
        .AddDefaultUI()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    services.AddDbContext<UniversityDBContext>(options =>
    {
        options.UseSqlServer(Configuration.GetConnectionString("UniversityDBConnection"));
    });

    // MVC with views
    //services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddControllersWithViews();

    // Razor Pages support is required for Identity's default UI
    services.AddRazorPages();

    RegisterServices(services);
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
async Task BuildAndRunAppAsync()
{
    var app = builder.Build();
    var env = app.Environment;
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        //app.UseDatabaseErrorPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCookiePolicy();

    // Routing must come before authentication/authorization for endpoint routing
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    // Map controllers and MVC route
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Map Razor Pages (required for Identity UI and Razor Pages in the project)
    app.MapRazorPages();

    app.Run();
}

static void RegisterServices(IServiceCollection services)
{
    DependencyContainer.RegisterServices(services);
}

ConfigureServices();
await BuildAndRunAppAsync();