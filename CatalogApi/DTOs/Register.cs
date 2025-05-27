using System.ComponentModel.DataAnnotations;

namespace CatalogApi.DTOs;

public class Register
{
    [Required(ErrorMessage = "Username Obrigatorio!")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Email Obrigatorio!")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password Obrigatorio!")]
    public string? Password { get; set; }
}