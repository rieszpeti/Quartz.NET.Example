using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Impl.AdoJobStore;
using Quartz.NET.Example.JobHelpers;
using Quartz.NET.Example.Jobs;
using Quartz.NET.Example.Quartz.Components;
using Quartz.NET.Example.Repository;
using Quartz.NET.Example.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example;

internal static class Startup
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
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

                // You can setup this from Options
                var schedName = q.SchedulerName = "SchedName";

                // For clustering this is essential
                q.SchedulerId = "SchedId";

                q.UseJobFactory<JobFactory>();

                // you can control whether job interruption happens for running jobs when scheduler is shutting down
                q.InterruptJobsOnShutdown = true;

                // when the program shuts down it will wait for the jobs to finish
                q.InterruptJobsOnShutdownWithWait = true;

                q.MaxBatchSize = 10;

                // This is the default it is for auto-interrupt long running jobs
                q.UseJobAutoInterrupt(options => options.DefaultMaxRunTime = TimeSpan.FromMinutes(5));

                q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 5);

                q.UsePersistentStore(persistentStore =>
                {
                    persistentStore.UseSqlServer(sqlServerOptions =>
                    {
                        sqlServerOptions.UseDriverDelegate<SqlServerDelegate>();

                        // You can get this also from options
                        sqlServerOptions.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Quartz.NET.Example;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;";

                        // This is the default it is for the quartz tables only
                        sqlServerOptions.TablePrefix = "QRTZ_";
                    });

                    // This will check the TablePrefix from the UseSqlServer
                    persistentStore.PerformSchemaValidation = true;

                    persistentStore.UseProperties = true;

                    persistentStore.UseNewtonsoftJsonSerializer();

                    persistentStore.UseClustering();
                });

                services.AddQuartzHostedService(quartzOptions => 
                {
                    quartzOptions.AwaitApplicationStarted = true;

                    // when the program shuts down it will wait for the jobs to finish
                    quartzOptions.WaitForJobsToComplete = true;

                    quartzOptions.StartDelay = TimeSpan.FromSeconds(1);
                });

#if DEBUG
                var jobName = "TEST";

                // Add job from program for test
                StartTestJob(q, typeof(DummyJob), jobName, "TriggerName");

                //StartTestJob(q, typeof(SomeJob), "TEST", "TriggerName");

                //StartTestJob(q, typeof(SomeJobWithNextJob), "TEST", "TriggerName");

                AddConfigToDB(schedName, jobName);
#endif
            });

            // Register services
            services.AddDbContext<QuartzNetExampleContext>();

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
        });

    private static void StartTestJob(IServiceCollectionQuartzConfigurator q, Type jobType, string jobName, string triggerName)
    {
        // Create key for the job
        var jobKey = new JobKey(jobName);

        q.AddJob(jobType, jobKey);

        q.AddTrigger(t => t.ForJob(jobKey)
                           .WithIdentity(triggerName)
                           .StartNow());
    }

    /// <summary>
    /// This is just for demonstration
    /// it is not a nice way to add data at startup
    /// </summary>
    private static void AddConfigToDB(string schedName, string jobName, string jobGroup = "DEFAULT")
    {
        var repo = new Repo(new QuartzNetExampleContext());

        repo.AddAtStartup(schedName, jobName, jobGroup);
    }
}
