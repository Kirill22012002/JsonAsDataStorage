using System.Dynamic;

namespace JsonAsDataStorage.Core;

public class BaseStorage<T> : IBaseStorage<T>
{
    private readonly string _filePath;
    private readonly string _idField;
    private Dictionary<string, T> _storage = new Dictionary<string, T>();

    public BaseStorage(string filePath, string idField)
    {
        _filePath = filePath;
        _idField = idField;
    }

    public virtual async Task<T> GetItemAsync(dynamic id)
    {
        return await GetItemAsync(GetFilterPredicate(id));
    }

    public virtual async Task<T> GetItemAsync(Predicate<T> filter)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(_filePath);
        if (existingList != null)
        {
            var item = existingList.FirstOrDefault(e => filter(e));
            if (item != null)
            {
                return item;
            }
        }
        throw new KeyNotFoundException($"Item not found");
    }

    public virtual async Task<IEnumerable<T>> GetAllItemsAsync()
    {
        return await JsonFileHelper.ReloadAsync<T>(_filePath);
    }

    public virtual async Task<bool> InsertItemAsync(T item)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(_filePath);
        if (existingList != null)
        {
            var list = existingList.ToList();
            list.Add(item);
            await JsonFileHelper.UploadAsync(_filePath, list);
            return true;
        }
        else
        {
            await JsonFileHelper.UploadAsync(_filePath, new List<T> { item });
            return true;
        }
    }

    public virtual async Task<bool> InsertListAsync(IEnumerable<T> items)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(_filePath);
        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            list.AddRange(items);
            await JsonFileHelper.UploadAsync(_filePath, list);
            return true;
        }
        else
        {
            await JsonFileHelper.UploadAsync(_filePath, items);
            return true;
        }
    }

    public async Task<bool> ReplaceItemAsync(dynamic id, T item)
    {
        return await ReplaceItemAsync(GetFilterPredicate(id), item);
    }

    public async Task<bool> ReplaceItemAsync(Predicate<T> filter, T item)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(_filePath);

        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            var idx = list.FindIndex(e => filter(e));
            if (idx != -1)
            {
                list[idx] = item;
                await JsonFileHelper.UploadAsync(_filePath, list);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> UpdateItemAsync(dynamic id, T item)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteItemAsync(dynamic id)
    {
        return await DeleteItemAsync(GetFilterPredicate(id));
    }

    public async Task<bool> DeleteItemAsync(Predicate<T> filter)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(_filePath);

        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            var idx = list.FindIndex(e => filter(e));
            if (idx != -1)
            {
                list.RemoveAt(idx);
                await JsonFileHelper.UploadAsync(_filePath, list);
                return true;
            }
        }
        return false;
    }

    public virtual async Task DeleteAllAsync()
    {
        await JsonFileHelper.UploadAsync(_filePath, new List<T>());
    }

    private Predicate<T> GetFilterPredicate(dynamic id)
        => (e => GetFieldValue(e, _idField) == id);

    private dynamic GetFieldValue(object source, string fieldName)
    {
        if (source is ExpandoObject srcExpando)
        {
            var srcExpandoDict = new Dictionary<string, dynamic>(srcExpando, StringComparer.OrdinalIgnoreCase);
            return srcExpandoDict.ContainsKey(fieldName) ? srcExpandoDict[fieldName] : null;
        }

        var srcProp = source.GetType().GetProperties().FirstOrDefault(p => string.Equals(p.Name, fieldName, StringComparison.OrdinalIgnoreCase));
        return srcProp?.GetValue(source, null);
    }
}
