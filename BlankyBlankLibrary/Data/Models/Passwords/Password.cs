using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.Passwords;

[Table("blanky_passwords")]
public class Password {

    [Key]
    public int Id { get; set; }

    public int? LegacyId { get; set; }

    public string Name { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public Subcategory Subcategory { get; set; } = null!;

    public bool UsCentric { get; set; }

    public ICollection<AlternateSpelling> AlternateSpellings { get; set; } = null!;

    public ICollection<ForbiddenWord> ForbiddenWords { get; set; } = null!;

    public ICollection<TailoredWord> TailoredWords { get; set; } = null!;

}