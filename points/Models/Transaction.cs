using System;
using points.Models.Requests;

namespace points.Models;

public class Transaction
{

    //Let domain entity handle it's itialization through the constructor
    public Transaction(TransactionRequest request, Guid? spendEventId = null)
    {
        Id = Guid.NewGuid();
        PayerName = request.Payer;
        Points = request.Points;
        Type = request.Points >= 0 ? TransactionType.Earn : TransactionType.Spend;
        Timestamp = request.Timestamp;
        OffsetTransactions = new List<Transaction>();
        SpendEventId = spendEventId;
    }

    public Guid Id { get; }

    //Transactions should be effectively immutable events - hence only getters on these properties

    public string PayerName { get; }

    public int Points { get; }

    public DateTime Timestamp { get; }

    public TransactionType Type { get; }

    // Add a flag to avoid querying for record in which already had their points fully spent
    public Boolean Spent { get; set; }

    // Keep track of spend transactions that offset this record's point value
    public IList<Transaction> OffsetTransactions { get; set; }

    // For accounting purposes, if spend event is associated with this record,
    // document it here with this property. Handling negative transaction
    // requests from a client (treating this as if that's a requirement) won't result in one
    public Guid? SpendEventId { get; }

    //Encapsulate domain entity related functionality within the entity itself

    public int GetRemainingValue()
    {
        var value = this.Points - Math.Abs(this.OffsetTransactions.Sum(t => t.Points));

        return value;
    }

    public Transaction CreateOffsetTransaction(int decrementAmount, Guid? spendEventId = null, DateTime? requestTimestamp = null)
    {
        DateTime timestamp = requestTimestamp != null ? (DateTime)requestTimestamp : DateTime.Now;

        var newRequest = new TransactionRequest
        {
            Payer = this.PayerName,
            Points = decrementAmount,
            Timestamp = timestamp,
        };

        var newTransaction = new Transaction(newRequest, spendEventId);
        this.OffsetTransactions.Add(newTransaction);

        return newTransaction;
    }
}

public enum TransactionType
{
    Earn,
    Spend
}

