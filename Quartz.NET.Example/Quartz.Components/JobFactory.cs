using Microsoft.Extensions.DependencyInjection;
using Quartz.NET.Example.Jobs;
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
        var jobDetail = bundle.JobDetail;

        var type = jobDetail.JobType;

        try
        {
            if (type == typeof(SomeJob))
            {
                var someService = _serviceProvider.GetRequiredService<SomeService>();

                return new SomeJob(someService);
            }
            else
            {
                // You can return whatever implements IJob
                return new DummyJob();
            }
        }
        catch (Exception ex)
        {
            return new ExceptionJob(ex);
        }
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}
