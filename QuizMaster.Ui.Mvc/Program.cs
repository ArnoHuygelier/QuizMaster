using Microsoft.EntityFrameworkCore;
using QuizMaster.Repository;
using QuizMaster.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString(nameof(QuizMasterDbContext));

builder.Services.AddDbContext<QuizMasterDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});

// Add services here
//builder.Services.AddScoped<FunctionService>();
builder.Services.AddScoped<QuizService>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
