using ModularCommerce.Abstractions;

namespace ModularCommerce.Pipelines;

public sealed class AuthorizationDecorator<TRequest,TResponse>(
    IHttpContextAccessor httpContextAccessor,
    IHandler<TRequest,TResponse> handler) : IHandler<TRequest,TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest command, CancellationToken cancellationToken)
    {
        if (command is not IUserScopeRequest scopedRequest)
            return await handler.HandleAsync(command, cancellationToken);
        
        var userIdClaim = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (userIdClaim is null )
            throw new UnauthorizedAccessException();
        
        scopedRequest.SetUserId(long.Parse(userIdClaim));
        return await handler.HandleAsync(command, cancellationToken);
    }
}
