namespace BlankyBlankLibrary.ViewModels;

public class StructureList {

    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public IList<string> Structures { get; set; } = null!;

}