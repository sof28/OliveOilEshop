using Azure.Core;
using ModularCommerce.Abstractions;

namespace ModularCommerce.Pipelines
{
    public sealed class LoggingDecorator<TRequest, TResponse>(ILogger<LoggingDecorator<TRequest,TResponse>> logger, IHandler<TRequest,TResponse> handler) : IHandler<TRequest,TResponse>
    {
        public async Task<TResponse> HandleAsync(TRequest command, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogInformation("Handling Request {RequestName}", requestName);

            var response = await handler.HandleAsync(command, cancellationToken);
            if (response is Result result && result.IsFailure)
            {
                logger.LogWarning(
                "Request {RequestName} failed with error code {ErrorCode}",
                requestName,
                result.Error.Code);
            }
            logger.LogInformation("Handled request {RequestName}", requestName);

            return response;
        }
    }
}

