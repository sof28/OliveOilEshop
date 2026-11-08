using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Abstractions;
using ModularCommerce.Abstractions.Errors;
using ModularCommerce.Constants;
using ModularCommerce.Database;
using ModularCommerce.Entities;
using ModularCommerce.Extensions;
using ModularCommerce.Pipelines;

namespace ModularCommerce.Features.WishlistFeature;

public sealed record WishlistItemResponse(
    long ProductId, string Name, string Slug, decimal Price, string? ImageUrl, DateTime AddedAt);

public sealed record WishlistChangeResponse(long ProductId, bool InWishlist);

public static class WishlistErrors
{
    public static Error ProductNotFound(long id) =>
        Error.NotFound("Wishlist.ProductNotFound", $"The product with id {id} was not found");
}

public sealed class GetWishlistRequest : IUserScopeRequest
{
    public long UserId { get; private set; }
    public void SetUserId(long userId) => UserId = userId;
}

public sealed class AddToWishlistRequest(long productId) : IUserScopeRequest
{
    public long ProductId { get; } = productId;
    public long UserId { get; private set; }
    public void SetUserId(long userId) => UserId = userId;
}

public sealed class RemoveFromWishlistRequest(long productId) : IUserScopeRequest
{
    public long ProductId { get; } = productId;
    public long UserId { get; private set; }
    public void SetUserId(long userId) => UserId = userId;
}

public sealed class GetWishlistHandler(ApplicationDbContext db)
    : IHandler<GetWishlistRequest, Result<List<WishlistItemResponse>>>
{
    public async Task<Result<List<WishlistItemResponse>>> HandleAsync(
        GetWishlistRequest command, CancellationToken cancellationToken)
    {
        var items = await db.WishlistItems
            .Where(w => w.UserId == command.UserId)
            .OrderByDescending(w => w.AddedAt)
            .Select(w => new WishlistItemResponse(
                w.ProductId, w.Product!.Name, w.Product.Slug, w.Product.Price, w.Product.ImageUrl, w.AddedAt))
            .ToListAsync(cancellationToken);
        return Result.Success(items);
    }
}

public sealed class AddToWishlistHandler(ApplicationDbContext db)
    : IHandler<AddToWishlistRequest, Result<WishlistChangeResponse>>
{
    public async Task<Result<WishlistChangeResponse>> HandleAsync(
        AddToWishlistRequest command, CancellationToken cancellationToken)
    {
        if (!await db.Products.AnyAsync(p => p.Id == command.ProductId, cancellationToken))
            return Result.Failure<WishlistChangeResponse>(WishlistErrors.ProductNotFound(command.ProductId));

        var exists = await db.WishlistItems.AnyAsync(
            w => w.UserId == command.UserId && w.ProductId == command.ProductId, cancellationToken);
        if (!exists)
        {
            db.WishlistItems.Add(new WishlistItem { UserId = command.UserId, ProductId = command.ProductId });
            await db.SaveChangesAsync(cancellationToken);
        }
        return Result.Success(new WishlistChangeResponse(command.ProductId, true));
    }
}

public sealed class RemoveFromWishlistHandler(ApplicationDbContext db)
    : IHandler<RemoveFromWishlistRequest, Result<WishlistChangeResponse>>
{
    public async Task<Result<WishlistChangeResponse>> HandleAsync(
        RemoveFromWishlistRequest command, CancellationToken cancellationToken)
    {
        await db.WishlistItems
            .Where(w => w.UserId == command.UserId && w.ProductId == command.ProductId)
            .ExecuteDeleteAsync(cancellationToken);
        return Result.Success(new WishlistChangeResponse(command.ProductId, false));
    }
}

internal sealed class WishlistEndpoints : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("wishlist", [Microsoft.AspNetCore.Authorization.Authorize] async (
            IHandler<GetWishlistRequest, Result<List<WishlistItemResponse>>> handler,
            CancellationToken ct) =>
            (await handler.HandleAsync(new GetWishlistRequest(), ct)).Match(
                onSuccess: r => Results.Ok(r), onFailure: e => Results.BadRequest(e)))
            .WithTags(ApiTags.Wishlist);

        app.MapPost("wishlist/{productId:long}", [Microsoft.AspNetCore.Authorization.Authorize] async (
            IHandler<AddToWishlistRequest, Result<WishlistChangeResponse>> handler,
            [FromRoute] long productId, CancellationToken ct) =>
            (await handler.HandleAsync(new AddToWishlistRequest(productId), ct)).Match(
                onSuccess: r => Results.Ok(r), onFailure: e => Results.BadRequest(e)))
            .WithTags(ApiTags.Wishlist);

        app.MapDelete("wishlist/{productId:long}", [Microsoft.AspNetCore.Authorization.Authorize] async (
            IHandler<RemoveFromWishlistRequest, Result<WishlistChangeResponse>> handler,
            [FromRoute] long productId, CancellationToken ct) =>
            (await handler.HandleAsync(new RemoveFromWishlistRequest(productId), ct)).Match(
                onSuccess: r => Results.Ok(r), onFailure: e => Results.BadRequest(e)))
            .WithTags(ApiTags.Wishlist);
    }
}
