using System;
using System.Collections.Generic;

namespace TrafficViolationApp.model;

public partial class Violation
{
    public int ViolationId { get; set; }

    public int ReportId { get; set; }

    public string PlateNumber { get; set; } = null!;

    public int? ViolatorId { get; set; }

    public decimal FineAmount { get; set; }

    public DateTime? FineDate { get; set; }

    public bool? PaidStatus { get; set; }

    public virtual Report Report { get; set; } = null!;

    public virtual User? Violator { get; set; }
}
