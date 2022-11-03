using System;
using System.ComponentModel.DataAnnotations;

namespace points.Models.Requests;

public class SpendEventRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int points { get; set; } 
}

