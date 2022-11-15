using DAL;
using DAL.Services;
using DataConvertor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
        {            
            options.UseSqlServer("server=DESKTOP-SABOU\\SQLEXPRESS2014;Database=ocr_management;User Id=ocrmng;Password=ocrmng",
                assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

        });
        services.AddScoped<IProjectService, ProjectService>();
        #region Quartz

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var jobKeyDoConvert = new JobKey("DoConvert");
            q.AddJob<DoConvert>(opts =>
            opts.WithIdentity(jobKeyDoConvert));

            q.AddTrigger(t => t.ForJob(jobKeyDoConvert)
            .WithIdentity(jobKeyDoConvert + " trigger").WithSimpleSchedule(c => c.WithIntervalInMinutes(30).RepeatForever())
            .StartAt(DateTimeOffset.Now.AddMinutes(1)));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        #endregion
    })
    .Build();

host.Run();




