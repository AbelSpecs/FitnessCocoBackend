using InsightCore.Application.Interface.UseCases;
using InsightCore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InsightCore.Persistence.Repositories
{
    public class JwtProvider(IConfiguration configuration) : IJwtProvider
    {
        public string GenerateToken(User user)
        {
            // 1. Definir los Claims (Información del usuario)
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new("FirstName", user.FirstName) // Custom claim
        };

            // 2. Agregar Roles
            // foreach (var role in user.Roles) { claims.Add(new Claim(ClaimTypes.Role, role.Name)); }

            // 3. Crear la llave de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Config:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Configurar el token
            var token = new JwtSecurityToken(
                issuer: configuration["Config:Issuer"],
                audience: configuration["Config:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
