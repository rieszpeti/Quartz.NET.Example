using Quartz.NET.Example.Models;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.JobHelpers;

internal class NextJobCreator(NextJobConfig nextJobConfig)
{
    /// <summary>
    /// This is just for the key value pairs
    /// </summary>
    public static readonly string NextJobName = "NextJobName";
    public static readonly string NextJobGroup = "NextJobGroup";
    public static readonly string NextJobType = "NextJobType";

    private readonly NextJobConfig _config = nextJobConfig;

    public bool CheckNextJob()
    {
        return !_config.NextJobName.IsNullOrWhiteSpace() || 
               !_config.NextJobType.IsNullOrWhiteSpace() || 
               !_config.NextJobGroup.IsNullOrWhiteSpace();
    }

    public void AddNextJob(IJobExecutionContext context)
    {
        //This MergedJobDataMap is a key value pair collection
        context.MergedJobDataMap.Put(NextJobName, _config.NextJobName);
        context.MergedJobDataMap.Put(NextJobGroup, _config.NextJobGroup);
        context.MergedJobDataMap.Put(NextJobType, _config.NextJobType);
    }
}
