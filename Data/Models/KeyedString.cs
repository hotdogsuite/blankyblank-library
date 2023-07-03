using System.ComponentModel.DataAnnotations;

namespace BlankyBlankLibrary.Data.Models;

public abstract class KeyedString {

    [Key]
    public int Id { get; set; }

    public string Value { get; set; } = null!;
    
}