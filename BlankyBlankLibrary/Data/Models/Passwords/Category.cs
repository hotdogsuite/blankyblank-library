using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.Passwords;

[Table("blanky_passwords_categories")]
public class Category {

    [Key]
    public int Id { get; set; }
    
    public string CategoryName { get; set; } = null!;

}