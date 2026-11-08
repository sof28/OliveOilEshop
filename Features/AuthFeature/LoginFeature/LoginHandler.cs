using ModularCommerce.Abstractions;
using ModularCommerce.Database;
using ModularCommerce.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ModularCommerce.Features.AuthFeature.Login;

namespace ModularCommerce.Features.AuthFeature.LoginFeature;

public sealed class LoginHandler(ApplicationDbContext dbContext, IConfiguration configuration) : IHandler<LoginRequest, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> HandleAsync(LoginRequest command, CancellationToken cancellationToken)
    {
        // Validate user
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == command.Username, cancellationToken);
        if (user == null)
        {
            return Result.Failure<LoginResponse>(AuthErrors.InvalidCredentials());
        }
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, command.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return Result.Failure<LoginResponse>(AuthErrors.InvalidCredentials());
        }

        // Generate JWT token
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

        return Result.Success(new LoginResponse { Token = tokenString });
    }
}

