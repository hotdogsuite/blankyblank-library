using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlankyBlankLibrary.Data.Models.WordLists;

[Table("blanky_wordlists")]
public class List {

    [Key]
    public int Id { get; set; }

    public int? LegacyId { get; set; }

    public string Name { get; set; } = null!;

    public int? Amount { get; set; }

    public int? MaxChoices { get; set; }

    public bool Optional { get; set; }

    public string? Placeholder { get; set; }

    public ICollection<ListWord> Words { get; set; } = null!;

    public ICollection<ListList> Lists { get; set; } = null!;

}