using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Impl.AdoJobStore;
using Quartz.NET.Example.JobHelpers;
using Quartz.NET.Example.Jobs;
using Quartz.NET.Example.Quartz.Components;
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

            services.AddQuartz(q =>
            {
                // You can setup this from Options
                q.SchedulerName = "SchedName";

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
                        sqlServerOptions.ConnectionString = "";

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
            });

            // Register services

            // Jobs
            services.AddTransient<DummyJob>();
            services.AddTransient<ExceptionJob>();
            services.AddTransient<SomeJob>();

            // JobHelpers
            services.AddTransient<NextJobCreator>();

            // Services
            services.AddScoped<SomeService>();

        });
}
