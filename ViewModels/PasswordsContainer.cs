using Newtonsoft.Json;

namespace BlankyBlankLibrary.ViewModels;

public class PasswordsContainer {

    [JsonProperty("content")]
    public IEnumerable<Password> Passwords { get; set; } = null!;

    public class Password {

        [JsonProperty("alternateSpellings")]
        public IEnumerable<string> AlternateSpellings { get; set; } = null!;

        [JsonProperty("category")]
        public string Category { get; set; } = null!;

        [JsonProperty("subcategory")]
        public string Subcategory { get; set; } = null!;

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; } = null!;

        [JsonProperty("forbiddenWords")]
        public IEnumerable<string> ForbiddenWords { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("password")]
        public string Name { get; set; } = null!;

        [JsonProperty("tailoredWords")]
        public IEnumerable<TailoredWord> TailoredWords { get; set; } = null!;
        
        [JsonProperty("us")]
        public bool Us { get; set; }
        
        public class TailoredWord {

            [JsonProperty("list")]
            public string List { get; set; } = null!;

            [JsonProperty("word")]
            public string Word { get; set; } = null!;

        }

    }

}