using Microsoft.IdentityModel.Tokens;
using TsDataAnnotations.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TsDataAnnotations.Server.Services;

public static class TokenService
{
    public static TokenModel GenerateToken (Usuario user, UsuariosRole [] roles)
    {
        JwtSecurityTokenHandler tokenHandler = new ();

        byte[] key = KeyVaultService.GetKeyBytes();

        TokenModel tokenModel = new()
        {
            Username = user.Username,
            Expires = DateTimeOffset.UtcNow.AddMinutes(5),
            Roles = roles.Select(x => x.Role).ToArray(),
        };

        List<Claim> listClaim = new();

        listClaim.Add(new Claim(ClaimTypes.Name, tokenModel.Username));
        listClaim.Add(new Claim(ClaimTypes.Expiration, tokenModel.Expires.ToUnixTimeSeconds().ToString()));

        foreach (string r in tokenModel.Roles)
        {
            listClaim.Add(new Claim(ClaimTypes.Role, r));
        }

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(listClaim.ToArray()),
            Expires = tokenModel.Expires.UtcDateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        tokenModel.Token = tokenHandler.WriteToken(token);

        return tokenModel;
    }
}