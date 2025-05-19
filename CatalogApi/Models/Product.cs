using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CatalogApi.Validations;

namespace CatalogApi.Models;

[Table("Products")]
public class Product : IValidatableObject
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    [FirstCapitalLetter]
    public string? Name { get; set; }
    [Required]
    [StringLength(300)]
    public string? Description { get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Category? Category { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (this.Name != null && !string.IsNullOrEmpty(this.Name.ToString()))
        {
            var firstCapital = this.Name.ToString()[0].ToString();

            if (firstCapital != firstCapital.ToUpper())
            {
                yield return new ValidationResult("A primeira letra deve ser maiuscula!", new[] { nameof(this.Name) });
            }
        }

        if (this.Stock < 0)
        {
            yield return new ValidationResult("O Estoque nao pode ser negativo!", new[] { nameof(this.Stock) });
        }
    }
}