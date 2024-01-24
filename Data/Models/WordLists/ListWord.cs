using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.WordLists;

[Table("blanky_wordlists_words")]
public class ListWord {

    [Key]
    public int Id { get; set; }

    public string WordName { get; set; } = null!;

    public bool AlwaysChoose { get; set; }

    public int ParentListId { get; set; }

    public List ParentList { get; set; } = null!;

}