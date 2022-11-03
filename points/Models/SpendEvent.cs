using System;
namespace points.Models;

public class SpendEvent
{
    public SpendEvent(int amount)
    {
        Id = Guid.NewGuid();
        Amount = amount;
    }

    public Guid Id { get; set; }
    public int Amount { get; set; }
}

