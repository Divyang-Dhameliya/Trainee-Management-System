using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace TraineeManagement.Api.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        string? value = user.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (value == null || !long.TryParse(value, out long userId))
        {
            throw new InvalidOperationException("Authenticated request is missing a valid user id claim.");
        }

        return userId;
    }
}