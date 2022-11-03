using System;
namespace points.Models;

public class Payer
{

    public Payer(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id;
    public string Name;
}

