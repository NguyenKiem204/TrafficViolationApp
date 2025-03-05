using System;
using System.Collections.Generic;

namespace TrafficViolationApp.model;

public partial class Report
{
    public int ReportId { get; set; }

    public int ReporterId { get; set; }

    public string ViolationType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PlateNumber { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }

    public string Location { get; set; } = null!;

    public DateTime? ReportDate { get; set; }

    public string? Status { get; set; }

    public int? ProcessedBy { get; set; }

    public virtual User? ProcessedByNavigation { get; set; }

    public virtual User Reporter { get; set; } = null!;

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
