using System;
using System.Collections.Generic;

namespace Quartz.NET.Example.Models.Quartz;

public partial class QrtzSimpleTrigger
{
    public string SchedName { get; set; } = null!;

    public string TriggerName { get; set; } = null!;

    public string TriggerGroup { get; set; } = null!;

    public int RepeatCount { get; set; }

    public long RepeatInterval { get; set; }

    public int TimesTriggered { get; set; }

    public virtual QrtzTrigger QrtzTrigger { get; set; } = null!;
}
