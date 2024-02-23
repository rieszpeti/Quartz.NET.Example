using System;
using System.Collections.Generic;

namespace Quartz.NET.Example.Models.Quartz;

public partial class QrtzPausedTriggerGrp
{
    public string SchedName { get; set; } = null!;

    public string TriggerGroup { get; set; } = null!;
}
