using Newtonsoft.Json;

namespace JsonAsDataStorage.Core;

public class JsonFileHelper
{
    public static async Task<IEnumerable<T>> ReloadAsync<T>(string filePath)
    {
        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (var reader = new StreamReader(fs))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }
        }
    }

    public static async Task UploadAsync<T>(string filePath, IEnumerable<T> items)
    {
        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            using (var writer = new StreamWriter(fs))
            {
                var json = JsonConvert.SerializeObject(items, Formatting.Indented);
                await writer.WriteAsync(json);
            }
        }
    }
}
