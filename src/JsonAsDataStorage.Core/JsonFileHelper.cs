using Newtonsoft.Json;
using System.Diagnostics;

namespace JsonAsDataStorage.Core;

public class JsonFileHelper
{
    public static async Task<IEnumerable<T>> ReloadAsync<T>(string filePath)
    {
        Stopwatch sw = null;
        var json = "{}";

        while (true)
        {
            try
            {
                json = await File.ReadAllTextAsync(filePath);
                break;
            }
            catch (IOException e) when (e.Message.Contains("because it is being used by another process"))
            {
                sw ??= Stopwatch.StartNew();
                if (sw.ElapsedMilliseconds > 10000)
                    throw;
            }
        }

        return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
    }

    public static async Task<bool> UploadAsync<T>(string filePath, IEnumerable<T> items)
    {
        Stopwatch sw = null;
        var json = JsonConvert.SerializeObject(items, Formatting.Indented);

        while (true)
        {
            try
            {
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (IOException e) when (e.Message.Contains("because it is being used by another process"))
            {
                sw ??= Stopwatch.StartNew();
                if (sw.ElapsedMilliseconds > 10000)
                    return false;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
 