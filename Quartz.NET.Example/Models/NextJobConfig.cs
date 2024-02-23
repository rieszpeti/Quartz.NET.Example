using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Models;


public class NextJobConfig
{
    /// <summary>
    /// Composite key
    /// </summary>
    public string SchedName { get; init; } = null!;
    public string JobName { get; init; } = null!;
    public string JobGroup { get; init; } = null!;

    public string SpecialProperty { get; init; } = null!;

    public string NextJobName { get; init; } = null!;
    public string NextJobGroup { get; init; } = "DEFAULT";
    public string NextJobType { get; init; } = null!;


    // One to one wiht JobConfig
    public virtual JobConfig JobConfig { get; set; } = null!;

    public NextJobConfig()
    {
        
    }

    public NextJobConfig(
        string schedName, string jobName, string nextJobName, string nextJobType, 
        string jobGroup = "DEFAULT", string nextJobGroup = "DEFAULT")
    {
        SchedName = schedName;
        JobName = jobName;
        JobGroup = jobGroup;
        NextJobName = nextJobName;
        NextJobGroup = nextJobGroup;
        NextJobType = nextJobType;
    }
}
