using System;
using System.ComponentModel.DataAnnotations;
using points.Attributes;

namespace points.Models.Requests;

public class TransactionRequest
{
    [Required]
    [StringLength(256, MinimumLength = 1)]
    public string Payer { get; set; }

    [Required]
    [PointsValidation]
    public int Points { get; set; }

    //Give the client the option not to specify the date/time
    public DateTime Timestamp { get; set; } = DateTime.Now;
}

