namespace BlankyBlankLibrary.ViewModels;

public class FileContainer {

    public byte[] FileContents { get; set; } = null!;

    public string FileContentType { get; set; } = null!;

    public string? FileName { get; set; }

}