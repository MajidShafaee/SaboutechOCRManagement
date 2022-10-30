



using WebApp.SignalRHubs;

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
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    //  hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
    // hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(1);

});
builder.Services.AddTransient<IProjectService, ProjectService>();

#region Quartz

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    //var jobKey = new JobKey("Add Legacy Data Job");
    //q.AddJob<AddLegacyDataJob>(opts =>
    //opts.WithIdentity(jobKey));

    //q.AddTrigger(t => t.ForJob(jobKey)
    //.WithIdentity(jobKey + " trigger")
    //.StartAt(DateTimeOffset.Now.AddMinutes(30)));

    //var jobKeyReadFiles = new JobKey("ReadProjectFileJob");
    //q.AddJob<ReadProjectFile>(opts =>
    //opts.WithIdentity(jobKeyReadFiles));

    //q.AddTrigger(t => t.ForJob(jobKeyReadFiles)
    //.WithIdentity(jobKeyReadFiles + " trigger")
    //.StartAt(DateTimeOffset.Now.AddMinutes(1)));

    var jobKeyDoFileOCR = new JobKey("DoFileOCR");
    q.AddJob<DoFileOCR>(opts =>
    opts.WithIdentity(jobKeyDoFileOCR));

    q.AddTrigger(t => t.ForJob(jobKeyDoFileOCR)
    .WithIdentity(jobKeyDoFileOCR + " trigger")//.WithSimpleSchedule(c=>c.WithIntervalInMinutes(45).RepeatForever())
    .StartAt(DateTimeOffset.Now.AddMinutes(1)));


    var jobKeyUpdateHomeInfo = new JobKey("UpdateHomeInfo");
    q.AddJob<UpdateHomeInfo>(opts =>
    opts.WithIdentity(jobKeyUpdateHomeInfo));

    q.AddTrigger(t => t.ForJob(jobKeyUpdateHomeInfo)
    .WithIdentity(jobKeyUpdateHomeInfo + " trigger").WithSimpleSchedule(c=>c.WithIntervalInMinutes(5).RepeatForever())
    .StartAt(DateTimeOffset.Now.AddMinutes(1)));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

#endregion

var app = builder.Build();

using (var db = new AppDbContext())
{
    
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

app.MapHub<InfoHub>("/infoHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using (var db = new AppDbContext())
{
   
        db.Database.Migrate();
}
