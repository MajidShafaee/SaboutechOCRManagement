



var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbConnStr"),
        assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
   
});

builder.Services.AddScoped<IProjectService, ProjectService>();

#region Quartz

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("Add Legacy Data Job");
    q.AddJob<AddLegacyDataJob>(opts =>
    opts.WithIdentity(jobKey));

    q.AddTrigger(t => t.ForJob(jobKey)
    .WithIdentity(jobKey + " trigger")
    .StartAt(DateTimeOffset.Now.AddMinutes(30)));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

#endregion
var app = builder.Build();
using (var db = new AppDbContext())
{
    if (db.Database.GetPendingMigrations().Count() > 0)
        db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
using (var db = new AppDbContext())
{
    if (db.Database.GetPendingMigrations().Count() > 0)
        db.Database.Migrate();
}