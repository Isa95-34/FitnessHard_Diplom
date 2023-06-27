using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessHard.Models;

public partial class Subscription
{
    internal string SubscriptionT;

    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsUnlimited { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public int LengthInMonths { get; set; }

    public decimal Price { get; set; }

    [NotMapped]
    public object SubscriptionType { get; set; }



    public virtual ICollection<ServicesSubscription> ServicesSubscriptions { get; set; } = new List<ServicesSubscription>();

    public virtual ICollection<SubscriptionClient> SubscriptionClients { get; set; } = new List<SubscriptionClient>();
    
}
