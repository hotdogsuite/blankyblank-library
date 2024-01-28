using System.ComponentModel.DataAnnotations;

namespace BlankyBlankLibrary.ViewModels;

public class PasswordEdit {

    public int Id { get; set; }

    [Display(Name = "Secret Prompt")]
    public string Name { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    [Display(Name = "Category")]
    public string CombinedCategoryIdentifier { get; set; } = null!;

    [Display(Name = "US-centric")]
    public bool UsCentric { get; set; }

    public IList<AlternateSpelling> AlternateSpellings { get; set; } = null!;

    public IList<ForbiddenWord> ForbiddenWords { get; set; } = null!;

    public IList<TailoredWord> TailoredWords { get; set; } = null!;
    
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