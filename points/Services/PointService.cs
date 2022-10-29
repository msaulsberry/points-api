using System;
using points.Models;
using points.Models.Requests;
using points.Models.Responses;
using points.Services.Interfaces;

namespace points.Services;

public class PointService : IPointService
{

    //mock the data stores for our entities
    public IList<Transaction> Transactions = new List<Transaction>();

    public IList<Payer> Payers = new List<Payer>();

    public IList<SpendEvent> SpendEvents = new List<SpendEvent>();


    public PointService() {}

    //publicly exposed interface

    public string? AddTransaction(TransactionRequest request)
    {
        var newTransaction = new Transaction(request);

        if (newTransaction.Type == TransactionType.Spend)
            return SpendPointsByPayer(request);

        Transactions.Add(newTransaction);

        if (!(Payers.Any(p => p.Name == newTransaction.PayerName)))
            Payers.Add(new Payer { Id = Guid.NewGuid(), Name = newTransaction.PayerName });

        return "success";
    }

    public IList<PayerSpendResponse>? SpendPoints(SpendEventRequest request)
    {
        var pointsToSpend = request.points;

        // Returning null is often not preferred, but for simplicity's sake, it represents an
        // invalid transaction that would result in negative points for a user
        if (pointsToSpend <= 0 || pointsToSpend > GetCurrentBalance())
            return null;

        var eligibleTransactions = Transactions
                                    .Where(t => !t.Spent && t.Type == TransactionType.Earn)
                                    .OrderBy(t => t.Timestamp).ToList();

        var spendEvent = new SpendEvent { Id = Guid.NewGuid(), Amount = request.points };
        var generatedTransactions = GenerateSpendTransactions(pointsToSpend, eligibleTransactions, spendEvent.Id);

        var spendResponses = new List<PayerSpendResponse>();
        foreach (var transaction in generatedTransactions)
        {
            spendResponses.Add(new PayerSpendResponse { payer = transaction.PayerName, points = transaction.Points });
        }

        return spendResponses;

    }

    public object GetPayerBalances()
    {
        var payerBalances = new Dictionary<string, int>();

        foreach (var payer in Payers)
        {
            var payerName = payer.Name;
            var balance = GetBalanceByPayer(payer.Name);
            payerBalances.Add(payerName, balance);
        }

        return payerBalances;
    }


    //private methods encapsulated in the service

    private string? SpendPointsByPayer(TransactionRequest request)
    {
        var pointsToSpend = Math.Abs(request.Points);

        if (pointsToSpend > GetBalanceByPayer(request.Payer))
            return null;

        var eligibleTransactions = Transactions
                                    .Where(t => !t.Spent && t.Type == TransactionType.Earn && t.PayerName == request.Payer)
                                    .OrderBy(t => t.Timestamp).ToList();

        var addedTransactions = GenerateSpendTransactions(pointsToSpend, eligibleTransactions);

        return "success";
    }

    private IList<Transaction> GenerateSpendTransactions(int pointsToSpend, IList<Transaction> eligibleTransactions, Guid? spendEventId = null)
    {
        var generatedTransactions = new List<Transaction>();

        foreach (var transaction in eligibleTransactions)
        {
            var remainingPositiveBalance = transaction.GetRemainingValue();
            Transaction newTransaction;

            if (pointsToSpend >= remainingPositiveBalance)
            {
                newTransaction = transaction.CreateOffsetTransaction(remainingPositiveBalance * -1, spendEventId);
                transaction.Spent = true;
                pointsToSpend -= remainingPositiveBalance;
            }
            else
            {
                // transaction has enough positive points to offset current spend 
                newTransaction = transaction.CreateOffsetTransaction(pointsToSpend * -1, spendEventId);
                pointsToSpend = 0;
            }

            generatedTransactions.Add(newTransaction);
            Transactions.Add(newTransaction);

            if (pointsToSpend == 0)
                break;
        }

        return generatedTransactions;
    }

    private int GetBalanceByPayer(string payerName)
    {
        var balance = Transactions.Where(t => t.PayerName == payerName).Sum(t => t.Points);

        return balance;
    }

    private int GetCurrentBalance()
    {
        var balance = Transactions.Sum(t => t.Points);

        return balance;
    }
}

