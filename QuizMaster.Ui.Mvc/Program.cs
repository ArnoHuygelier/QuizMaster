using Microsoft.EntityFrameworkCore;
using QuizMaster.Repository;
using Microsoft.AspNetCore.Identity;
using QuizMaster.Models;
using QuizMaster.Ui.Mvc.Helpers;

using Microsoft.Data.SqlClient;
using System.Data;
using QuizMaster.Services.Services;
using QuizMaster.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString(nameof(QuizMasterDbContext));

builder.Services.AddDbContext<QuizMasterDbContext>(options =>
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string is empty");
    }
    options.UseSqlServer(connectionString);
});

builder.Services.AddDefaultIdentity<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<QuizMasterDbContext>();

// Add services here
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IdentityRole>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<IAvatarService, AvatarService>();
builder.Services.AddScoped<IUserBadgeService, UserBadgeService>();
builder.Services.AddScoped<IQuizResultService, QuizResultService>();
builder.Services.AddScoped<IHintService, HintService>();
builder.Services.AddScoped<IEndGameService, EndGameService>();
builder.Services.AddScoped<EndGameService>();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetService<QuizMasterDbContext>();

    if (!dbcontext.Database.CanConnect())
    {
        throw new InvalidOperationException("Cannot connect to the database, check the connection string.");
    }
}


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
//app.MapControllerRoute(
//    name: "profile",
//    pattern: "{naam}",
//    defaults: new { controller = "Users", action = "Profile" });

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
