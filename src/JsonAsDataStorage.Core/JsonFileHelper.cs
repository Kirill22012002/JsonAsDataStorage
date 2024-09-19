using Newtonsoft.Json;

namespace JsonAsDataStorage.Core;

public class JsonFileHelper
{
    public static async Task<IEnumerable<T>> ReloadAsync<T>(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
    }

    public static async Task UploadAsync<T>(string filePath, IEnumerable<T> items)
    {
        var json = JsonConvert.SerializeObject(items, Formatting.Indented);
        await File.WriteAllTextAsync(filePath, json);
    }
}
