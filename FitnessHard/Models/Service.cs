using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Price { get; set; }

    public string Information { get; set; } = null!;

    public virtual ICollection<JournalService> JournalServices { get; set; } = new List<JournalService>();

    public virtual ICollection<ServicesSubscription> ServicesSubscriptions { get; set; } = new List<ServicesSubscription>();
}
