using Newtonsoft.Json;

namespace BlankyBlankLibrary.JsonModels;

public class SentenceStructuresModel {

    [JsonProperty("content")]
    public IList<JetContent> Content { get; set; } = null!;

    public class JetContent {

        [JsonProperty("category")]
        public string Category { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("structures")]
        public IList<string> Structures { get; set; } = null!;

    }

}