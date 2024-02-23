using Microsoft.Extensions.DependencyInjection;
using Quartz.NET.Example.JobHelpers;
using Quartz.NET.Example.Jobs;
using Quartz.NET.Example.Repository;
using Quartz.NET.Example.Services;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Quartz.Components;

/// <summary>
/// This class will create the actual jobs
/// We will use service locator pattern to create jobs using the built in ServiceProvider
/// </summary>
internal class JobFactory(IServiceProvider serviceProvider) : IJobFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var jobName = "TEST";
        var schedName = "SchedName";

        var jobDetail = bundle.JobDetail;

        var type = jobDetail.JobType;

        try
        {
            if (type == typeof(SomeJob))
            {
                var someService = _serviceProvider.GetRequiredService<SomeService>();

                return new SomeJob(someService);
            }
            if (type == typeof(SomeJobWithNextJob))
            {
                //So if a job or the service that is used by it is highly dependent on a service then you have to inject it like this

                var repo = _serviceProvider.GetRequiredService<Repo>();

                repo.AddAtStartup(schedName, jobName);

                var jobConfig = repo.GetJobConfig(schedName, jobName);

                if (jobConfig is null || jobConfig.NextJobConfig is null)
                {
                    throw new InvalidOperationException("JobConfig or NextJobConfig is null.");
                }

                return new SomeJobWithNextJob(new NextJobCreator(jobConfig.NextJobConfig));
            }
            else
            {
                // You can return whatever implements IJob
                return new DummyJob();
            }
        }
        catch
        {
            // if an init is failed the most of the time Quartz can't handle it so you have to add this error job or do nothing
            return new ErrorJob();
        }
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}
