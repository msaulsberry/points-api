using System;
using System.ComponentModel.DataAnnotations;
using points.Models.Requests;

namespace points.Attributes;

public class PointsValidationAttribute : ValidationAttribute
{
    public PointsValidationAttribute() {}

    public string GetErrorMessage() => "Invalid value for points";


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var pointsValue = (int?)value;

        if (pointsValue == null || pointsValue == 0)
            return new ValidationResult(GetErrorMessage());

        return ValidationResult.Success;

    }
}

