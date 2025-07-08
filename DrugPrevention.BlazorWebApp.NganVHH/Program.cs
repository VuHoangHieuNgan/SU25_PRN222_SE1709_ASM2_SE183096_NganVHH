using DrugPrevention.BlazorWebApp.NganVHH.Components;
using DrugPrevention.BlazorWebApp.NganVHH.Components.Account;
using DrugPrevention.Repositories.NganVHH;
using DrugPrevention.Services.NganVHH;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Cần thiết để truy cập HttpContext trong các component Blazor (cho việc SignInAsync)
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Đăng ký từng service
builder.Services.AddScoped<ISystemUserAccountService, SystemUserAccountService>();
builder.Services.AddScoped<IUsersTuyenTMService, UsersTuyenTMService>();
builder.Services.AddScoped<IAppointmentsNganVHHService, AppointmentsNganVHHService>();
builder.Services.AddScoped<IConsultantsTrongLHService, ConsultantsTrongLHService>();


// Đăng ký ServiceProvider cuối cùng
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/Account/Login/GoogleResponse";
        options.SaveTokens = true;
    });

// Add authorization
builder.Services.AddAuthorization();


//builder.Services.AddAntiforgery();
builder.Services.AddHttpClient<HttpClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7115");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// THÊM ENDPOINT NÀY
app.MapPost("/login-handler", async (
    HttpContext httpContext,
    [FromForm] string userName,
    [FromForm] string password,
    // Inject service của bạn trực tiếp vào endpoint
    [FromServices] DrugPrevention.Services.NganVHH.IServiceProviders _serviceProviders) =>
{
    var userAccount = await _serviceProviders.SystemUserAccountService.GetUserAccountAsync(userName, password);

    if (userAccount != null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userAccount.UserName),
            new Claim(ClaimTypes.Role, userAccount.RoleId.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties { IsPersistent = true });

        // Đăng nhập thành công, chuyển hướng về trang chính
        return Results.Redirect("/AppointmentsNganVHHs/AppointmentsNganVHHList", true);
    }
    else
    {
        // Đăng nhập thất bại, chuyển hướng lại trang login với thông báo lỗi
        var errorMessage = Uri.EscapeDataString("Login fail, please check your account.");
        return Results.Redirect($"/Account/Login?ErrorMessage={errorMessage}");
    }
});
// ----------------

app.MapPost("/logout-handler", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/Account/Logout");
});

app.Run();