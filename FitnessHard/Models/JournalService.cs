using System;
using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class JournalService
{
    public int Id { get; set; }

    public int SubscriptionClientId { get; set; }

    public int ServiceId { get; set; }

    public DateTime DateUse { get; set; }

    public decimal Cost { get; set; }

    public int EmployeeId { get; set; }

    public int TrainerId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual SubscriptionClient SubscriptionClient { get; set; } = null!;

    public virtual Employee Trainer { get; set; } = null!;
}