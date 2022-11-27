using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Repositories;
using Store.Data.Repositories.IRepositories;
using Store.Utilities.Braintree;

var builder = WebApplication.CreateBuilder(args);
var connectionName = "DefaultConnection";
var connectionString = builder
    .Configuration
    .GetConnectionString(connectionName)??
    throw new InvalidOperationException($"Connection string {connectionName} not found.");

var googleAuthSection = builder
    .Configuration.GetSection("Web");

var brainTreeSection = builder
    .Configuration.GetSection("BrainTree");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.Configure<BraintreeSettings>(brainTreeSection);
builder.Services.AddSingleton<IBraintreeGate, BraintreeGate>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IInquiryHeaderRepository,InquiryHeaderRepository>();
builder.Services.AddScoped<IInquiryDetailsRepository, InquiryDetailsRepository>();
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = googleAuthSection["client_id"]!;
    googleOptions.ClientSecret = googleAuthSection["client_secret"]!;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout=TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly= true;
    options.Cookie.IsEssential= true;
});


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
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();