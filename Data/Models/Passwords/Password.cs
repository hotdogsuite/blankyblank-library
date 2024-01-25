using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.Passwords;

[Table("blanky_passwords")]
public class Password {

    [Key]
    public int Id { get; set; }

    public int? LegacyId { get; set; }

    public string Name { get; set; } = null!;

}