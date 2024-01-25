using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.Passwords;

[Table("blanky_passwords_tailoredwords")]
public class TailoredWord {

    [Key]
    public int Id { get; set; }
    
    public string Word { get; set; } = null!;

    public int ListId { get; set; }

    public WordLists.List List { get; set; } = null!;

    public int PasswordId { get; set; }

    public Password Password { get; set; } = null!;

}