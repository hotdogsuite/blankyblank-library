using Newtonsoft.Json;

namespace BlankyBlank.Models;

public class WordListsModel {

    [JsonProperty("content")]
    public List<JetContent> Content { get; set; } = null!;

    public class JetContent {

        [JsonProperty("amount")]
        public string Amount { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("maxChoices")]
        public string MaxChoices { get; set; } = null!;

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("optional")]
        public bool Optional { get; set; }

        [JsonProperty("placeholder")]
        public string Placeholder { get; set; } = null!;

        [JsonProperty("words")]
        public List<JetWord> Words { get; set; } = null!;

        public class JetWord {

            [JsonProperty("alwaysChoose")]
            public bool AlwaysChoose { get; set; }

            [JsonProperty("word")]
            public string Word { get; set; } = null!;

        }

    }

}