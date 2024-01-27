using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.Passwords;

[Table("blanky_passwords_forbiddenwords")]
public class ForbiddenWord {

    [Key]
    public int Id { get; set; }
    
    public string Word { get; set; } = null!;

    public int PasswordId { get; set; }

    public Password Password { get; set; } = null!;

}