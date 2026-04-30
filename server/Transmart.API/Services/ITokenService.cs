using TranSmart.Core.Result;
using System.Collections.Generic;
using System.Security.Claims;

namespace TranSmart.API.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateAccessToken(IEnumerable<Claim> claims, string key);
        string GenerateRefreshToken();
        Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token, string key);
		bool IsTokenExpired(string token,string key);
		bool IsTokenExpired(string token);

	}
}
