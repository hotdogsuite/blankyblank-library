using Newtonsoft.Json;

namespace BlankyBlankLibrary.JsonModels;

public class FolderDataModel {

    [JsonProperty("fields")]
    public IList<Field> Fields { get; set; } = null!;

    public class Field {

        [JsonProperty("t")]
        public string Type { get; set; } = null!;

        [JsonProperty("v")]
        public string Value { get; set; } = null!;

        [JsonProperty("n")]
        public string Name { get; set; } = null!;
        
    }

}