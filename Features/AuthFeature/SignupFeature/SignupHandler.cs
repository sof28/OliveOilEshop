using ModularCommerce.Abstractions;
using ModularCommerce.Database;
using ModularCommerce.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ModularCommerce.Features.AuthFeature.Signup;

namespace ModularCommerce.Features.AuthFeature.SignupFeature;

public sealed class SignupHandler(ApplicationDbContext dbContext, IConfiguration configuration) : IHandler<SignupRequest, Result<SignupResponse>>
{
    public async Task<Result<SignupResponse>> HandleAsync(SignupRequest command, CancellationToken cancellationToken)
    {
        // Check if user exists
        if (await dbContext.Users.AnyAsync(u => u.Username == command.Username || u.Email == command.Email, cancellationToken))
        {
            return Result.Failure<SignupResponse>(AuthErrors.UserAlreadyExists());
        }

        // Create user and hash password
        var user = new User
        {
            Username = command.Username,
            Password = string.Empty, // Will be set after hashing
            Email = command.Email,
            Name = command.Name,
            Surname = command.Surname,
            IsActive = true
        };
        var hasher = new PasswordHasher<User>();
        user.Password = hasher.HashPassword(user, command.Password);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Generate JWT token (reuse login logic)
        var jwtSettings = configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Result.Success(new SignupResponse { Token = tokenString });
    }
}



