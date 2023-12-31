using Newtonsoft.Json;

namespace BlankyBlankLibrary.JsonModels;

public class PasswordsModel {

    [JsonProperty("content")]
    public JetContent[] Content { get; set; } = null!;

    public class JetContent {

        [JsonProperty("alternateSpellings")]
        public IList<string> AlternateSpellings { get; set; } = null!;

        [JsonProperty("category")]
        public string Category { get; set; } = null!;

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; } = null!;

        [JsonProperty("forbiddenWords")]
        public IList<string> ForbiddenWords { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("password")]
        public string Password { get; set; } = null!;

        [JsonProperty("subcategory")]
        public string Subcategory { get; set; } = null!;

        [JsonProperty("tailoredWords")]
        public IList<JetTailoredWord> TailoredWords { get; set; } = null!;

        [JsonProperty("us")]
        public bool Us { get; set; }

        public class JetTailoredWord {

            [JsonProperty("list")]
            public string List { get; set; } = null!;

            [JsonProperty("word")]
            public string Word { get; set; } = null!;
            
        }
    }

}