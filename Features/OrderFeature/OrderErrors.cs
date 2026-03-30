using OliveOilEshop.Abstractions.Errors;

namespace OliveOilEshop.Features.OrderFeature
{
    public static class OrderErrors
    {
        public static Error NotFound(long id) => Error.NotFound("Order.NotFound", $"The Order with Id '{id}' was not found");
    }
}
