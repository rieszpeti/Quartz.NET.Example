using Quartz.NET.Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Repository;

/// <summary>
/// It is not a nice repository so don't use this pattern
/// here is a nice one: https://www.youtube.com/watch?v=kwehxBDX_o8&t=890s
/// </summary>
/// <param name="ctx"></param>
internal class Repo(QuartzNetExampleContext ctx)
{
    private readonly QuartzNetExampleContext _ctx = ctx;

    public void AddAtStartup(string schedName, string jobName, string jobGroup = "DEFAULT")
    {
        var isJobConfExists = !_ctx.JobConfigs.Any(j => j.SchedName == schedName &&
                                                        j.JobName == jobName &&
                                                        j.JobGroup == jobGroup);

        if (isJobConfExists)
        {
            var jobConfig = new JobConfig
            {
                SchedName = schedName,
                JobName = jobName,
                JobGroup = jobGroup,
                Name = "SomeName1"
            };

            _ctx.JobConfigs.Add(jobConfig);
        }

        var isNextJConfExists = !_ctx.NextJobConfigs.Any(nj => nj.SchedName == schedName &&
                                                               nj.JobName == jobName &&
                                                               nj.JobGroup == jobGroup);

        if (isNextJConfExists)
        {
            var nextJobConfig = new NextJobConfig
            {
                SchedName = schedName,
                JobName = jobName,
                JobGroup = jobGroup,
                SpecialProperty = "Special",
                NextJobName = jobName + "NextJobName",
                NextJobType = "DummyJob"
            };

            _ctx.NextJobConfigs.Add(nextJobConfig);
        }

        SaveChanges();
    }

    public JobConfig? GetJobConfig(string schedName, string jobName, string jobGroup = "DEFAULT")
    {
        return _ctx.JobConfigs.FirstOrDefault(j =>
                j.SchedName == schedName &&
                j.JobName == jobName &&
                j.JobGroup == jobGroup);
    }

    public void SaveChanges() => _ctx.SaveChanges();
}