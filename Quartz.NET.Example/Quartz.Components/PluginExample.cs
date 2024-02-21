using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Quartz.Components;

/// <summary>
/// It is boilerplate to create jobListener
/// </summary>
internal class PluginExample : ISchedulerPlugin
{
    public Task Initialize(string pluginName, IScheduler scheduler, CancellationToken cancellationToken = default)
    {
        scheduler.ListenerManager.AddJobListener(new JobListener(), EverythingMatcher<JobKey>.AllJobs());
        return Task.CompletedTask;
    }

    public Task Shutdown(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Start(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
