using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ProductDemo.Web.Helpers.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductDemo.Web.Helpers;

public class JwtHelper
{
    private readonly JwtSettings _jwtSettings;
    public JwtHelper(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }
    public string GetUserIdFromJwt(HttpRequest request)
    {
        var token = request.Cookies["AccessToken"];
        var handler = new JwtSecurityTokenHandler();

        var jwtToken = handler.ReadJwtToken(token);
        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        return userId;
    }

    public string? ExtractTokenFromHeader(HttpRequest request)
    {
        if (request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            var token = authorizationHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            return string.IsNullOrEmpty(token) ? null : token;
        }
        return null;
    }

    public async Task<string> GenerateJwtTokenAsync(int? userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.AccessKey);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(), ClaimValueTypes.Integer32),
            new Claim("UserId", userId.ToString()!),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddSeconds(_jwtSettings.AccessTokenExpiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return await Task.FromResult(jwtToken);
    }

}
