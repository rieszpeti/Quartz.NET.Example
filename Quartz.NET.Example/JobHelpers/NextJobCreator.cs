using Quartz.NET.Example.Models;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.JobHelpers;

internal class NextJobCreator
{
    /// <summary>
    /// This is just for the key value pairs
    /// </summary>
    public static readonly string NextJobName = "NextJobName";
    public static readonly string NextJobGroup = "NextJobGroup";
    public static readonly string NextJobType = "NextJobType";

    private readonly NextJobConfig _config;

    public NextJobCreator(NextJobConfig nextJobConfig)
    {
        _config = nextJobConfig;
    }

    public bool CheckNextJob(string nextJobName, string nextJobType, string nextJobGroup = "DEFAULT")
    {
        return !nextJobName.IsNullOrWhiteSpace() || !nextJobType.IsNullOrWhiteSpace() || !nextJobGroup.IsNullOrWhiteSpace();
    }

    public void AddNextJob(IJobExecutionContext context)
    {
        //This MergedJobDataMap is a key value pair collection
        context.MergedJobDataMap.Put(NextJobName, _config.NextJobName);
        context.MergedJobDataMap.Put(NextJobName, _config.NextJobGroup);
        context.MergedJobDataMap.Put(NextJobName, _config.NextJobType);
    }
}
