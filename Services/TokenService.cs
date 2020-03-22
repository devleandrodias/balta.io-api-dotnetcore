using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shop;
using Shop.Models;

namespace Shop.Services
{
  public static class TokenService
  {
    public static string GenerateToken(User user)
    {
      JwtSecurityTokenHandler tokenHandle = new JwtSecurityTokenHandler();

      byte[] key = Encoding.ASCII.GetBytes(Settings.Secret);

      SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[] {
          new Claim(ClaimTypes.Name, user.Username.ToString()),
          new Claim(ClaimTypes.Role, user.Role.ToString())
        }),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha384Signature)
      };

      SecurityToken token = tokenHandle.CreateToken(tokenDescriptor);

      return tokenHandle.WriteToken(token);
    }
  }
}
