using System;
using System.Collections.Generic;

namespace Quartz.NET.Example.Models.Quartz;

public partial class QrtzJobDetail
{
    public string SchedName { get; set; } = null!;

    public string JobName { get; set; } = null!;

    public string JobGroup { get; set; } = null!;

    public string? Description { get; set; }

    public string JobClassName { get; set; } = null!;

    public bool IsDurable { get; set; }

    public bool IsNonconcurrent { get; set; }

    public bool IsUpdateData { get; set; }

    public bool RequestsRecovery { get; set; }

    public byte[]? JobData { get; set; }

    public virtual ICollection<QrtzTrigger> QrtzTriggers { get; set; } = new List<QrtzTrigger>();

    // Added manually
    public virtual JobConfig? JobConfig { get; set; }
}
