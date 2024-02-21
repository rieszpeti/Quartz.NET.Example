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

        string? nextJobName = dataMap.GetString("NextJobName");
        string? nextJobGroup = dataMap.GetString("NextJobGroup");
        string? nextJobType = dataMap.GetString("NextJobType");

        if (!nextJobName.IsNullOrWhiteSpace() &&
            !nextJobGroup.IsNullOrWhiteSpace() &&
            !nextJobType.IsNullOrWhiteSpace())
        {
            Type? jobType = Type.GetType("Quartz.NET.Example.Jobs." + nextJobType);

            if (jobType is null)
            {
                throw new Exception("Couldn't create job.");
            }

            var jobKey = new JobKey(nextJobName, nextJobGroup);

            var jobDetail = context.Scheduler.GetJobDetail(jobKey).GetAwaiter().GetResult();

            if (jobDetail is null)
            {
                throw new Exception("Couldn't create job.");
            }

            var trigger = TriggerBuilder.Create()
                .WithIdentity(nextJobName + "SimpleTrigger", nextJobGroup + "SimpleTriggerGroup")
                .ForJob(jobDetail)
                .StartNow()
                .Build();

            //Finally schedule job
            context.Scheduler.ScheduleJob(trigger).Wait();

        }

        return Task.CompletedTask;
    }
}
