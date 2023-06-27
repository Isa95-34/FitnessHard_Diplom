using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeSpan TimeStart { get; set; }

    public TimeSpan TimeEnd { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
