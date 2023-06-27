using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class ServicesAbonement
{
    public int AbonementId { get; set; }

    public int ServiceId { get; set; }

    public int Count { get; set; }

    public virtual Subscription Abonement { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
