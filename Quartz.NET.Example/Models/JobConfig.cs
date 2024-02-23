using Quartz.NET.Example.Models.Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.NET.Example.Models;

/// <summary>
/// It is a POCO class from the DB
/// </summary>
public class JobConfig
{
    /// <summary>
    /// Composite key
    /// </summary>
    public string SchedName { get; init; } = null!;
    public string JobName { get; init; } = null!;
    public string JobGroup { get; init; } = null!;

    public string Name { get; init; } = null!;

    public string? Description { get; set; }

    public string? SomeProp1 { get; set; }

    public string? SomeProp2 { get; set; }

    public string? JobProperty {  get; set; }

    // Imagine there are lot of properties here...

    /// <summary>
    /// One to one with NextJobConfig of course you can have one to many, 
    /// but you need to modify NextJobCreator to handle that
    /// </summary>
    public virtual NextJobConfig? NextJobConfig { get; set; }

    // you should add this class to the QrtzJobDetail as well
    public virtual QrtzJobDetail QrtzJobDetail { get; set; } = null!;

    public JobConfig()
    {
        
    }

    public JobConfig(string schedName, string jobName, string name, string jobGroup = "DEFAULT")
    {
        SchedName = schedName;
        JobName = jobName;
        JobGroup = jobGroup;
        Name = name;
    }


}
