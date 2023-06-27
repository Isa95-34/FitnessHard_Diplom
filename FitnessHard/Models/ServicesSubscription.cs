using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class ServicesSubscription
{
    public int SubscriptionId { get; set; }

    public int ServiceId { get; set; }

    public int Count { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;
}
