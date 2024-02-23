using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Impl.AdoJobStore;
using Quartz.NET.Example.JobHelpers;
using Quartz.NET.Example.Jobs;
using Quartz.NET.Example.Models;
using Quartz.NET.Example.Quartz.Components;
using Quartz.NET.Example.Repository;
using Quartz.NET.Example.Services;

namespace Quartz.NET.Example;

internal static class Startup
{
    private static string ConnectionString { get; set; } = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Quartz.NET.Example;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;";

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            ConfigureQuartz(hostContext.Configuration, services);

            RegisterServices(services);
        });

    private static void ConfigureQuartz(IConfiguration configuration, IServiceCollection services)
    {
        services.Configure<QuartzOptions>(options =>
        {
            options.Scheduling.IgnoreDuplicates = true;         // default : false
            options.Scheduling.OverWriteExistingData = true;    // default : true
        });

        // for setup
        // https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/microsoft-di-integration.html#di-aware-job-factories
        services.AddQuartz(q =>
        {
            ConfigureQuartzOptions(q);

            ConfigurePersistentStore(q);

            // Add test jobs if in debug mode
            // you can create and trigger jobs from here,
            // of course there are other ways
#if DEBUG
            var jobName = "TEST";
            var triggerName = "TriggerName";
            
            StartTestJob(q, typeof(DummyJob), jobName, triggerName);

            // StartTestJob(q, typeof(SomeJob), jobName, triggerName);

            // StartTestJob(q, typeof(SomeJobWithNextJob), jobName, triggerName);
#endif
        });

        services.AddQuartzHostedService(quartzOptions =>
        {
            quartzOptions.AwaitApplicationStarted = true;
            // when the program shuts down it will wait for the jobs to finish
            quartzOptions.WaitForJobsToComplete = true;
            quartzOptions.StartDelay = TimeSpan.FromSeconds(1);
        });
    }

    private static void ConfigureQuartzOptions(IServiceCollectionQuartzConfigurator q)
    {
        // You can setup this from Options
        q.SchedulerName = "SchedName";

        // For clustering this is essential
        q.SchedulerId = "SchedId";

        q.UseJobFactory<JobFactory>();
        // Register job listeners
        q.AddJobListener<JobListener>();
        q.InterruptJobsOnShutdown = true;
        q.InterruptJobsOnShutdownWithWait = true;
        q.MaxBatchSize = 10;
        q.UseJobAutoInterrupt(options => options.DefaultMaxRunTime = TimeSpan.FromMinutes(5));
        q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 5);
    }

    private static void ConfigurePersistentStore(IServiceCollectionQuartzConfigurator q)
    {
        q.UsePersistentStore(persistentStore =>
        {
            persistentStore.UseSqlServer(sqlServerOptions =>
            {
                sqlServerOptions.UseDriverDelegate<SqlServerDelegate>();
                sqlServerOptions.ConnectionString = ConnectionString;
                sqlServerOptions.TablePrefix = "QRTZ_";
            });

            // This will check the TablePrefix from the UseSqlServer
            persistentStore.PerformSchemaValidation = true;
            persistentStore.UseProperties = true;
            persistentStore.UseNewtonsoftJsonSerializer();
            persistentStore.UseClustering();
        });
    }

    private static void StartTestJob(IServiceCollectionQuartzConfigurator q, Type jobType, string jobName, string triggerName)
    {
        var jobKey = new JobKey(jobName);

        q.AddJob(jobType, jobKey);
        
        q.AddTrigger(t => t.ForJob(jobKey)
                           .WithIdentity(triggerName)
                           .StartNow());
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<QuartzNetExampleContext>(options => options.UseSqlServer(ConnectionString));

        // Repository
        services.AddTransient<Repo>();

        // Jobs
        services.AddTransient<DummyJob>();
        services.AddTransient<ErrorJob>();
        services.AddTransient<SomeJob>();

        // JobHelpers
        services.AddTransient<NextJobCreator>();

        // Services
        services.AddScoped<SomeService>();

        // add because of migration
        // Models
        services.AddTransient<JobConfig>();
        services.AddTransient<NextJobConfig>();
    }
}