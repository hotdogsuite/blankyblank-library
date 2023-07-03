using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models;

[Table("bbl_structures")]
public class SentenceStructure {

    [Key]
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public int? ImportedId { get; set; }

    public IList<StructureStructure> Structures { get; set; } = null!;

    [Table("bbl_structures_structures")]
    public class StructureStructure {

        [Key]
        public int Id { get; set; }

        public string Value { get; set; } = null!;

    }

}