using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.Passwords;

[Table("blanky_passwords_alternatespellings")]
public class AlternateSpelling {

    [Key]
    public int Id { get; set; }
    
    public string Spelling { get; set; } = null!;

    public int PasswordId { get; set; }

    public Password Password { get; set; } = null!;

}