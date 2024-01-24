using System.Text;

namespace BlankyBlankLibrary.Extensions;

public static class IFormFileExtensions {
  
    public static async Task<string> ReadFileContents (this IFormFile formFile) {
        var contents = new StringBuilder();
        using (var reader = new StreamReader(formFile.OpenReadStream())) {
            while (reader.Peek() >= 0) contents.AppendLine(await reader.ReadLineAsync());
        }
        return contents.ToString();
    }

}