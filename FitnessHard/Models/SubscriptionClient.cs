using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class SubscriptionClient
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int SubscriptionId { get; set; }

    public DateTime DateStart { get; set; } = DateTime.Now;

    public DateTime DateEnd { get; set; } = DateTime.Now.AddDays(1);

    public int EmployeeId { get; set; }

    public bool Status { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<JournalService> JournalServices { get; set; } = new List<JournalService>();

    public virtual Subscription Subscription { get; set; } = null!;

}
