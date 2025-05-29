using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using APICatalogo.Services;
using CatalogApi.DTOs;
using CatalogApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CatalogApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(Login login)
    {
        User? user = await _userManager.FindByNameAsync(login.Username!);

        if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password!))
        {
            return Unauthorized("Usuario ou senha invalidos!");
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

        JwtSecurityToken token = _tokenService.GenerateAccessToken(claims, _configuration);

        string refreshToken = _tokenService.GenerateRefreshToken();

        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

        await _userManager.UpdateAsync(user);

        return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = refreshToken, Expiration = token.ValidTo });
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(Register register)
    {
        User? user = await _userManager.FindByNameAsync(register.Username!);

        if (user != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Erro", Message = "Usuario ja existe!" });
        }

        User registeredUser = new()
        {
            Email = register.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = register.Username
        };

        var result = await _userManager.CreateAsync(registeredUser, register.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Erro", Message = "Criaçaõ do usuario falhou!" });
        }

        return Ok(new Response { Status = "Ok", Message = "Usuario Criado com sucesso!" });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(Token token)
    {
        ClaimsPrincipal claimsPrincipal = _tokenService.GetClaimsPrincipalFromExpiredAccessToken(token.AccessToken!, _configuration);

        if (claimsPrincipal == null)
        {
            return BadRequest();
        }

        User? user = await _userManager.FindByNameAsync(claimsPrincipal.Identity.Name);

        if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest();
        }

        JwtSecurityToken accessToken = _tokenService.GenerateAccessToken(claimsPrincipal.Claims.ToList(), _configuration);

        string refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        return new ObjectResult(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken), RefreshToken = refreshToken });
    } 
}