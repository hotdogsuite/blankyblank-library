using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankyBlankLibrary.Data.Models.WordLists;

[Table("blanky_wordlists_lists")]
public class ListList {

    [Key]
    public int Id { get; set; }

    public int ListId { get; set; }

    public List List { get; set; } = null!;

    public int ParentListId { get; set; }

    public List ParentList { get; set; } = null!;

    public bool AlwaysChoose { get; set; }

}