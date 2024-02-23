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
        if (!_ctx.JobConfigs.Any(j => j.JobName == "SomeName1"))
        {
            var jobConfig = new JobConfig(
                schedName,
                jobName,
                "SomeName1",
                jobGroup);

            _ctx.JobConfigs.Add(jobConfig);
        }

        if (!_ctx.JobConfigs.Any(j => j.JobName == "SomeName2"))
        {
            var jobConfig = new JobConfig(
                schedName,
                jobName,
                "SomeName2",
                jobGroup);

            _ctx.JobConfigs.Add(jobConfig);
        }

        if (!_ctx.NextJobConfigs.Any(nj => nj.JobName == "SomeName1" && nj.NextJobName == "DummyJob"))
        {
            var nextJobConfig = new NextJobConfig(
                schedName,
                jobName,
                jobName + "NextJobName",
                "DummyJob",
                jobGroup,
                jobGroup);

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
