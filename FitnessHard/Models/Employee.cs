using System;
using System.Collections.Generic;

namespace FitnessHard.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<JournalService> JournalServiceEmployees { get; set; } = new List<JournalService>();

    public virtual ICollection<JournalService> JournalServiceTrainers { get; set; } = new List<JournalService>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<SubscriptionClient> SubscriptionClients { get; set; } = new List<SubscriptionClient>();
}
