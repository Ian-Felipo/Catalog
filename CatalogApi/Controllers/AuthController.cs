using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CatalogApi.DTOs;
using CatalogApi.Models;
using CatalogApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CatalogApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(ITokenService tokenService, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _tokenService = tokenService;
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        User? user = await _userManager.FindByNameAsync(loginRequest.Username!);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password!))
        {
            return BadRequest();
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        string accessToken = new JwtSecurityTokenHandler().WriteToken(_tokenService.GenerateAccessToken(claims, _configuration));

        string refreshToken = _tokenService.GenerateRefreshToken();

        int refreshTokenValidityInMinutes = int.Parse(_configuration["JWT:RefreshTokenValidityInMinutes"]!);

        user.RefreshToken = refreshToken;

        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

        await _userManager.UpdateAsync(user);

        LoginResponse loginResponse = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
        return Ok(loginResponse);
    }
}