using Microsoft.EntityFrameworkCore;
using QuizMaster.Repository;
using QuizMaster.Services;
using Microsoft.AspNetCore.Identity;
using QuizMaster.Models;
using QuizMaster.Ui.Mvc.Helpers;
using QuizMaster.Services.Interfaces;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString(nameof(QuizMasterDbContext));

builder.Services.AddDbContext<QuizMasterDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});

builder.Services.AddDefaultIdentity<User>(options => 
     options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<QuizMasterDbContext>();

// Add services here
builder.Services.AddScoped<LeaderboardService>();
builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IdentityRole>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AnswerService>();
builder.Services.AddScoped<GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
	using var scope = app.Services.CreateScope();

	var dbContext = scope.ServiceProvider.GetRequiredService<QuizMasterDbContext>();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllerRoute(
    name: "profile",
    pattern: "{naam}",
    defaults: new { controller = "Users", action = "Profile" });

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Seed the database with roles
using (var scope = app.Services.CreateScope())
{
	await SeedDatabaseHelper.SeedRolesAndAdmin(scope.ServiceProvider);
}

app.MapRazorPages();

app.Run();
