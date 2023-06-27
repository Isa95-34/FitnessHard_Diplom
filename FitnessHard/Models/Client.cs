using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime Dob { get; set; } = DateTime.Now;

    public virtual ICollection<SubscriptionClient> SubscriptionClients { get; set; } = new List<SubscriptionClient>();
}
