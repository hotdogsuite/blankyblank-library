using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models;

[Table("bbl_passwords")]
public class Password {

    [Key]
    public int Id { get; set; }

    public IList<AlternateSpelling> AlternateSpellings { get; set; } = null!;

    public SentenceStructure Category { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public IList<ForbiddenWord> ForbiddenWords { get; set; } = null!;

    public int? ImportedId { get; set; }

    public string PasswordPassword { get; set; } = null!;

    public string? Subcategory { get; set; }

    public IList<TailoredWord> TailoredWords { get; set; } = null!;

    public bool UsCentric { get; set; }

    [Table("bbl_passwords_alt")]
    public class AlternateSpelling : KeyedString {

    }

    [Table("bbl_passwords_forbidden")]
    public class ForbiddenWord : KeyedString {

    }

    [Table("bbl_passwords_tailored")]
    public class TailoredWord {

        [Key]
        public int Id { get; set; }

        public string List { get; set; } = null!;

        public string Word { get; set; } = null!;

    }

}