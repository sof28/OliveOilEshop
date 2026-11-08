using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModularCommerce.Abstractions;
using ModularCommerce.Abstractions.Errors;
using ModularCommerce.Constants;
using ModularCommerce.Database;
using ModularCommerce.Entities;
using ModularCommerce.Extensions;
using ModularCommerce.Pipelines;

namespace ModularCommerce.Features.AccountFeature;

public sealed record AccountResponse(
    long Id, string Username, string Email, string Name, string Surname,
    string? Country, string? City, string? Address, string? PostalCode, string? PhoneNumber);

public static class AccountErrors
{
    public static Error NotFound() => Error.NotFound("Account.NotFound", "The account was not found");
    public static Error WrongPassword() => Error.Validation("Account.WrongPassword", "The current password is incorrect");
    public static Error EmailTaken() => Error.Conflict("Account.EmailTaken", "The email is already in use");
}

public sealed class GetAccountRequest : IUserScopeRequest
{
    public long UserId { get; private set; }
    public void SetUserId(long userId) => UserId = userId;
}

public sealed class UpdateAccountRequest : IUserScopeRequest
{
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public long UserId { get; private set; }
    public void SetUserId(long userId) => UserId = userId;
}

public sealed class ChangePasswordRequest : IUserScopeRequest
{
    public string CurrentPassword { get; set; } = "";
    public string NewPassword { get; set; } = "";
    public long UserId { get; private set; }
    public void SetUserId(long userId) => UserId = userId;
}

public sealed record ChangePasswordResponse(bool Changed);

public class UpdateAccountValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(100);
    }
}

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
    }
}

internal static class AccountMapping
{
    public static AccountResponse ToResponse(User u) => new(
        u.Id, u.Username, u.Email, u.Name, u.Surname,
        u.Country, u.City, u.Address, u.PostalCode, u.PhoneNumber);
}

public sealed class GetAccountHandler(ApplicationDbContext db)
    : IHandler<GetAccountRequest, Result<AccountResponse>>
{
    public async Task<Result<AccountResponse>> HandleAsync(GetAccountRequest command, CancellationToken ct)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == command.UserId, ct);
        return user is null
            ? Result.Failure<AccountResponse>(AccountErrors.NotFound())
            : Result.Success(AccountMapping.ToResponse(user));
    }
}

public sealed class UpdateAccountHandler(ApplicationDbContext db)
    : IHandler<UpdateAccountRequest, Result<AccountResponse>>
{
    public async Task<Result<AccountResponse>> HandleAsync(UpdateAccountRequest command, CancellationToken ct)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, ct);
        if (user is null)
            return Result.Failure<AccountResponse>(AccountErrors.NotFound());

        if (await db.Users.AnyAsync(u => u.Email == command.Email && u.Id != user.Id, ct))
            return Result.Failure<AccountResponse>(AccountErrors.EmailTaken());

        user.Email = command.Email.Trim();
        user.Name = command.Name.Trim();
        user.Surname = command.Surname.Trim();
        user.Country = command.Country;
        user.City = command.City;
        user.Address = command.Address;
        user.PostalCode = command.PostalCode;
        user.PhoneNumber = command.PhoneNumber;
        await db.SaveChangesAsync(ct);
        return Result.Success(AccountMapping.ToResponse(user));
    }
}

public sealed class ChangePasswordHandler(ApplicationDbContext db)
    : IHandler<ChangePasswordRequest, Result<ChangePasswordResponse>>
{
    public async Task<Result<ChangePasswordResponse>> HandleAsync(ChangePasswordRequest command, CancellationToken ct)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, ct);
        if (user is null)
            return Result.Failure<ChangePasswordResponse>(AccountErrors.NotFound());

        var hasher = new PasswordHasher<User>();
        if (hasher.VerifyHashedPassword(user, user.Password, command.CurrentPassword) == PasswordVerificationResult.Failed)
            return Result.Failure<ChangePasswordResponse>(AccountErrors.WrongPassword());

        user.Password = hasher.HashPassword(user, command.NewPassword);
        await db.SaveChangesAsync(ct);
        return Result.Success(new ChangePasswordResponse(true));
    }
}

internal sealed class AccountEndpoints : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("account/me", [Authorize] async (
            IHandler<GetAccountRequest, Result<AccountResponse>> handler, CancellationToken ct) =>
            (await handler.HandleAsync(new GetAccountRequest(), ct)).Match(
                onSuccess: r => Results.Ok(r), onFailure: e => Results.BadRequest(e)))
            .WithTags(ApiTags.Account);

        app.MapPut("account/me", [Authorize] async (
            IHandler<UpdateAccountRequest, Result<AccountResponse>> handler,
            UpdateAccountRequest request, CancellationToken ct) =>
            (await handler.HandleAsync(request, ct)).Match(
                onSuccess: r => Results.Ok(r), onFailure: e => Results.BadRequest(e)))
            .WithTags(ApiTags.Account);

        app.MapPost("account/change-password", [Authorize] async (
            IHandler<ChangePasswordRequest, Result<ChangePasswordResponse>> handler,
            ChangePasswordRequest request, CancellationToken ct) =>
            (await handler.HandleAsync(request, ct)).Match(
                onSuccess: r => Results.Ok(r), onFailure: e => Results.BadRequest(e)))
            .WithTags(ApiTags.Account);
    }
}
