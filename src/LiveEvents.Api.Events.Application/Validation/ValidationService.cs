using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Events.Application.Validation;

public sealed class ValidationService(IServiceProvider serviceProvider) : IValidationService
{
    public async Task<Result> ValidateAsync<T>(
        T instance,
        string errorCode,
        string errorMessage,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var validator = serviceProvider.GetService<IValidator<T>>();
        if (validator is null)
        {
            return Result.Success();
        }

        var validationResult = await validator.ValidateAsync(instance, cancellationToken);
        if (validationResult.IsValid)
        {
            return Result.Success();
        }

        return Result.Failure(Error.Validation(errorCode, errorMessage, ToDictionary(validationResult)));
    }

    private static Dictionary<string, string[]> ToDictionary(ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray());
    }
}
