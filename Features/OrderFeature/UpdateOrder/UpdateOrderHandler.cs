using ModularCommerce.Abstractions;
using ModularCommerce.Abstractions.Errors;
using ModularCommerce.Entities;
using ModularCommerce.Repository;
using System.Reflection.Metadata.Ecma335;

namespace ModularCommerce.Features.OrderFeature.UpdateOrder
{
    public sealed class UpdateOrderHandler(IRepository<Order> orderRepository, IUnitOfWork unitOfWork) : IHandler<UpdateOrderRequest, Result<UpdateOrderResponse>>
    {
        public async Task<Result<UpdateOrderResponse>> HandleAsync(UpdateOrderRequest command, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(command.Id, cancellationToken);
            if (order is null)
                return Result.Failure<UpdateOrderResponse>(OrderErrors.NotFound(command.Id));

            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = command.Price ?? order.TotalPrice;
            order.CustomerAddress = command.CustomerAddress ?? order.CustomerAddress;
            order.CustomerEmail = command.CustomerEmail ?? order.CustomerEmail;
            order.CustomerName = command.CustomerName ?? order.CustomerName;
            order.OrderItems = [.. command.OrderItems.Select(oi => new OrderItem
            {
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            })];
            order.UserId = command.UserId ?? order.UserId;

            await orderRepository.UpdateAsync(order, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            var response = new UpdateOrderResponse(
                  order.Id,
                  order.OrderDate,
                  order.TotalPrice,
                  order.CustomerAddress,
                  order.CustomerEmail,
                  order.CustomerName,
                  [.. order.OrderItems.Select(oi => new OrderItem
                  {
                      ProductId = oi.ProductId,
                      Quantity = oi.Quantity,
                      UnitPrice = oi.UnitPrice
                  })],
                  order.UserId
            );
            return Result.Success(response);
        }
    }
}
