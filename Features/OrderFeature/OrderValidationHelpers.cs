using OliveOilEshop.Entities;
using OliveOilEshop.Repository;

namespace OliveOilEshop.Features.OrderFeature
{
    public static class OrderValidationHelpers
    {
        public static async Task<bool> UserExistsAsync(long userId, IRepository<User> userRepository, CancellationToken ct)
        {
            var user = await userRepository.GetByIdAsync((int)userId, ct);
            return user != null;
        }
    }
}
