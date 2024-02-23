using Quartz.NET.Example.JobHelpers;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Quartz.Components;

internal class JobListener : IJobListener
{
    public string Name => "JobListener";

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        // it will be called before the job gets executed
        // you can prevent some issues here before the job gets executed etc.
        return Task.CompletedTask;
    }

    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// It is a nice method
    /// you can link jobs together from here like this
    /// </summary>
    /// <param name="context"></param>
    /// <param name="jobException"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
    {
        var dataMap = context.MergedJobDataMap;

        bool successNextJobName = dataMap.TryGetString(NextJobCreator.NextJobName, out string? nextJobName);

        bool successNextJobGroup = dataMap.TryGetString(NextJobCreator.NextJobGroup, out string? nextJobGroup);

        bool successNextJobType = dataMap.TryGetString(NextJobCreator.NextJobType, out string? nextJobType);

        if (successNextJobName && 
            successNextJobGroup && 
            successNextJobType &&
            !string.IsNullOrEmpty(nextJobName) &&
            !string.IsNullOrEmpty(nextJobGroup) &&
            !string.IsNullOrEmpty(nextJobType))
        {
            Type? jobType = Type.GetType("Quartz.NET.Example.Jobs." + nextJobType);

            if (jobType is null)
            {
                throw new Exception("Couldn't create job.");
            }

            IJobDetail job = JobBuilder.Create(jobType)
                                       .WithIdentity(nextJobName, nextJobGroup)
                                       .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                             .ForJob(job)
                                             .WithIdentity(nextJobName + "SimpleTrigger", nextJobGroup + "SimpleTriggerGroup")
                                             .StartNow()
                                             .Build();

            context.Scheduler.ScheduleJob(job, trigger, cancellationToken).GetAwaiter().GetResult();
        }

        return Task.CompletedTask;
    }
}
