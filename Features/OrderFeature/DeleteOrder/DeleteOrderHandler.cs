using OliveOilEshop.Abstractions;
using OliveOilEshop.Entities;
using OliveOilEshop.Repository;

namespace OliveOilEshop.Features.OrderFeature.DeleteOrder
{
    public class DeleteOrderHandler(IRepository<Order> orderRepository, IUnitOfWork unitOfWork) : IHandler<DeleteOrderRequest, Result<DeleteOrderResponse>>
    {
        public async Task<Result<DeleteOrderResponse>> HandleAsync(DeleteOrderRequest command, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(command.Id, cancellationToken);
            if (order == null)
            {
                return Result.Failure<DeleteOrderResponse>(OrderErrors.NotFound(command.Id));
            }

            await orderRepository.DeleteAsync(order, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(new DeleteOrderResponse(order.Id));
        }
    }
}
