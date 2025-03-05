using System;
using System.Collections.Generic;

namespace TrafficViolationApp.model;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public string PlateNumber { get; set; } = null!;

    public int OwnerId { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public int? ManufactureYear { get; set; }

    public virtual User Owner { get; set; } = null!;
}
