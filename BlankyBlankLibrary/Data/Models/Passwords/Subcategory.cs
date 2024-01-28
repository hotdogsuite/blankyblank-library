using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.Passwords;

[Table("blanky_passwords_subcategories")]
public class Subcategory {

    [Key]
    public int Id { get; set; }
    
    public string? SubcategoryName { get; set; } = null!;

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

}