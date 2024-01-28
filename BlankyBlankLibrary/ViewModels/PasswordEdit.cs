using System.ComponentModel.DataAnnotations;

namespace BlankyBlankLibrary.ViewModels;

public class PasswordEdit {

    public int Id { get; set; }

    [Display(Name = "Include in Export")]
    public bool IncludeInExport { get; set; }

    [Display(Name = "Secret Prompt")]
    public string Name { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    [Display(Name = "Category")]
    public int SubcategoryId { get; set; }

    [Display(Name = "US-centric")]
    public bool UsCentric { get; set; }

    public IList<AlternateSpelling> AlternateSpellings { get; set; } = new List<AlternateSpelling>();

    public IList<ForbiddenWord> ForbiddenWords { get; set; } = new List<ForbiddenWord>();

    public IList<TailoredWord> TailoredWords { get; set; } = new List<TailoredWord>();
    
    public class AlternateSpelling {

        public int Id { get; set; }

        public bool DeleteOnSave { get; set; }
        
        public string Spelling { get; set; } = null!;

    }
    
    public class ForbiddenWord {

        public int Id { get; set; }

        public bool DeleteOnSave { get; set; }
        
        public string Word { get; set; } = null!;

    }

    public class TailoredWord {

        public int Id { get; set; }

        public bool DeleteOnSave { get; set; }

        public string Word { get; set; } = null!;

        public int ListId { get; set; }

    }

}