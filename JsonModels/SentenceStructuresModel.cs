using Newtonsoft.Json;

namespace BlankyBlank.Models;

public class SentenceStructuresModel {

    [JsonProperty("content")]
    public List<JetContent> Content { get; set; } = null!;

    public class JetContent {

        [JsonProperty("category")]
        public string Category { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("structures")]
        public List<string> Structures { get; set; } = null!;

    }

}