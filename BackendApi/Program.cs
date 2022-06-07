using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedModel.Contexts;
using SharedModel.Helpers;
using SharedModel.Repository;
using SharedModel.Servers;
using SharedModel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddCors();
services.AddHttpClient<IHttpFactory, HttpFactory>();
services.AddScoped<IHttpFactory, HttpFactory>();
services.AddScoped<IAuthRepository, AuthRepository>();
services.AddScoped<IMailService, MailService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IPaymentRepository, PaymentRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<ICartRepository, CartRepository>();
services.AddScoped<IAlertRepository, AlertRepository>();



services.AddIdentity<Appuser, IdentityRole>(s =>
{
    s.User.RequireUniqueEmail = true;
    s.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<MainContext>()
.AddDefaultTokenProviders();

services.ConfigureApplicationCookie(options =>
{

    // Cookie settings
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = "nazAuth";

        options.Cookie.Domain = ".thenazrana.in"; // ".mydomain.com"

    options.Cookie.HttpOnly = false;

    options.ExpireTimeSpan = TimeSpan.FromDays(5 * 30);
    options.SlidingExpiration = true;
    //options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;

    options.LoginPath = new PathString("/Login");
    options.LogoutPath = new PathString("/Singout");
    options.AccessDeniedPath = new PathString("/Account/Login");


});

services.AddAuthentication();


builder.Services.AddControllersWithViews().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
});


services.AddDbContextPool<MainContext>(s => s.UseSqlServer(Settings.databaseString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseCors(s =>
{
    s.SetIsOriginAllowed(s => true);
    s.AllowAnyHeader();
    s.AllowAnyMethod();
    //s.AllowAnyOrigin();
    s.AllowCredentials();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()||true)
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

