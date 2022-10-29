using System;
using points.Models.Requests;
using points.Models.Responses;

namespace points.Services.Interfaces;

public interface IPointService
{
    string? AddTransaction(TransactionRequest request);

    IList<PayerSpendResponse> SpendPoints(SpendEventRequest request);

    object GetPayerBalances();
}

