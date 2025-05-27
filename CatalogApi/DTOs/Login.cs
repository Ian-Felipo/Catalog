using System.ComponentModel.DataAnnotations;

namespace CatalogApi.DTOs;

public class Login
{
    [Required(ErrorMessage = "O nome do usuario eh obrigatorio")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "A senha do usuario eh obrigatorio")]
    public string? Password { get; set; }
}