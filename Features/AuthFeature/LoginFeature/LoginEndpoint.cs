using OliveOilEshop.Abstractions;
using OliveOilEshop.Extensions;

namespace OliveOilEshop.Features.AuthFeature.Login;

internal sealed class LoginEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (IHandler<LoginRequest,Result<LoginResponse>> handler, LoginRequest command, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(command, cancellationToken);
            return result.Match(
                onSuccess: Results.Ok,
                onFailure: Results.BadRequest
            );
        })
        .WithTags("Auth")
        .Produces<LoginResponse>(statusCode: StatusCodes.Status200OK)
        .Produces(statusCode: StatusCodes.Status400BadRequest);
        
    }
}
