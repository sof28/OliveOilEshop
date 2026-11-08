using ModularCommerce.Abstractions;
using ModularCommerce.Constants;
using ModularCommerce.Extensions;

namespace ModularCommerce.Features.ContactFeature.SubmitContact;

internal sealed class SubmitContactEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("contact", async (
            IHandler<SubmitContactRequest, Result<SubmitContactResponse>> handler,
            SubmitContactRequest request,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            return result.Match(
                onSuccess: response => Results.Created($"/contact/{response.Id}", response),
                onFailure: error => Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Contact)
        .Produces<SubmitContactResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .AllowAnonymous();
    }
}
