using OliveOilEshop.Abstractions;
using OliveOilEshop.Extensions;

namespace OliveOilEshop.Features.AuthFeature.Signup;

internal sealed class SignupEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/auth/signup", async (IHandler<SignupRequest,Result<SignupResponse>> handler, SignupRequest command, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(command, cancellationToken);
            return result.Match(
                onSuccess: Results.Ok,
                onFailure: Results.BadRequest
            );
        })
        .WithTags("Auth")
        .Produces<SignupResponse>(statusCode: StatusCodes.Status200OK)
        .Produces(statusCode: StatusCodes.Status400BadRequest);
    }
}

