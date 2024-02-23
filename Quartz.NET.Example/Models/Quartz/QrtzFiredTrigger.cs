using System;
using System.Collections.Generic;

namespace Quartz.NET.Example.Models.Quartz;

public partial class QrtzFiredTrigger
{
    public string SchedName { get; set; } = null!;

    public string EntryId { get; set; } = null!;

    public string TriggerName { get; set; } = null!;

    public string TriggerGroup { get; set; } = null!;

    public string InstanceName { get; set; } = null!;

    public long FiredTime { get; set; }

    public long SchedTime { get; set; }

    public int Priority { get; set; }

    public string State { get; set; } = null!;

    public string? JobName { get; set; }

    public string? JobGroup { get; set; }

    public bool? IsNonconcurrent { get; set; }

    public bool? RequestsRecovery { get; set; }
}
