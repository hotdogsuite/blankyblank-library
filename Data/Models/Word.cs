using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models;

[Table("bbl_words")]
public class Word {

    [Key]
    public int Id { get; set; }

    public int? ImportedId { get; set; }

    public int? Amount { get; set; }

    public int? MaxChoices { get; set; }

    public string Name { get; set; } = null!;

    public bool Optional { get; set; }

    public string? Placeholder { get; set; }

    public IList<WordWord> Words { get; set; } = null!;

    [Table("bbl_words_words")]
    public class WordWord {

        [Key]
        public int Id { get; set; }

        public bool AlwaysChoose { get; set; }

        public string Word { get; set; } = null!;

        public Word? LinkedWord { get; set; } = null!;

    }

}