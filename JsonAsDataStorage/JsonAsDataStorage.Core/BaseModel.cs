using Newtonsoft.Json;

namespace JsonAsDataStorage.Core;

public class BaseModel
{
    [JsonProperty("id")] public string Id { get; set; }
}
