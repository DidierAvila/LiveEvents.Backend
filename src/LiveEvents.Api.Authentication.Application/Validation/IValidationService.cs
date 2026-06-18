using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.Validation;

public interface IValidationService
{
    Task<Result> ValidateAsync<T>(
        T instance,
        string errorCode,
        string errorMessage,
        CancellationToken cancellationToken = default);
}
