using System;
namespace points.Models.Requests;

public class TransactionRequest
{
    public string Payer { get; set; }
    public int Points { get; set; }
    public DateTime Timestamp { get; set; }
}

