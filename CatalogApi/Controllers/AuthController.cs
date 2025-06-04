using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CatalogApi.DTOs;
using CatalogApi.Models;
using CatalogApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        User? user = await _userManager.FindByNameAsync(registerRequest.Username!);

        if (user != null)
        {
            return BadRequest();
        }

        User registeredUser = new User
        {
            UserName = registerRequest.Username,
            Email = registerRequest.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(registeredUser, registerRequest.Password!);

        RegisterResponse registerResponse = new RegisterResponse
        {
            Status = result.Succeeded ? "Success" : "Error",
            Message = result.Succeeded ? "User Created" : "User Creation Failed"
        };

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, registerResponse);
        }

        return Ok(registerResponse);
    }

    [HttpPost("Refresh-Token")]
    public async Task<IActionResult> RefreshToken(LoginResponse loginResponse)
    {
        ClaimsPrincipal claims = _tokenService.GetClaimsPrincipalFromExpiredAccessToken(loginResponse.AccessToken!, _configuration);

        string username = claims.Identity!.Name!;

        User? user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != loginResponse.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest();
        }

        string accessToken = new JwtSecurityTokenHandler().WriteToken(_tokenService.GenerateAccessToken(claims.Claims.ToList(), _configuration));

        string refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        LoginResponse newLoginResponse = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Ok(newLoginResponse);
    }

    [Authorize]
    [HttpPost("Revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        User? user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return BadRequest();
        }

        user.RefreshToken = null;

        await _userManager.UpdateAsync(user);

        return Ok();
    }
}