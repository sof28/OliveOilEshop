using OliveOilEshop.Abstractions.Errors;

namespace OliveOilEshop.Features.AuthFeature;

internal static class AuthErrors
{
    internal static Error InvalidCredentials() => Error.Validation(
        "InvalidCredentials",
        "The provided username or password is incorrect.");
    
    internal static Error UserNotFound() => Error.NotFound("UserNotFound", "The specified user was not found.");
    internal static Error UserAlreadyExists() => Error.Conflict("UserAlreadyExists", "The specified user already exists.");
}