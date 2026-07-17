using FluentValidation.Results;

namespace PortfolioTracker.Api.Extensions;

public static class ValidationExtensions
{
    public static IDictionary<string, string[]> ToErrorDictionary(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());
    }
}
