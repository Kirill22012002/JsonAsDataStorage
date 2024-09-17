namespace JsonAsDataStorage.Core;

public class BaseStorage<T> : IBaseStorage<T> where T : BaseModel
{
    public string FilePath { get; set; }

    public virtual async Task<T> GetItemAsync(string id)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(FilePath);
        if (existingList != null)
        {
            var item = existingList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                return item;
            }
        }
        throw new KeyNotFoundException($"Item with id {id} not found");
    }

    public virtual async Task<IEnumerable<T>> GetAllItemsAsync()
    {
        return await JsonFileHelper.ReloadAsync<T>(FilePath);
    }

    public virtual async Task<bool> InsertItemAsync(T item)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(FilePath);
        if (existingList != null)
        {
            var list = existingList.ToList();
            list.Add(item);
            await JsonFileHelper.UploadAsync(FilePath, list);
            return true;
        }
        else
        {
            await JsonFileHelper.UploadAsync(FilePath, new List<T> { item });
            return true;
        }
    }

    public virtual async Task<bool> InsertListAsync(IEnumerable<T> items)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(FilePath);
        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            list.AddRange(items);
            await JsonFileHelper.UploadAsync(FilePath, list);
            return true;
        }
        else
        {
            await JsonFileHelper.UploadAsync(FilePath, items);
            return true;
        }
    }

    public async Task<bool> ReplaceItemAsync(string id, T item)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(FilePath);

        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            var idx = list.FindIndex(x => x.Id == id);
            if (idx != -1)
            {
                list[idx] = item;
                await JsonFileHelper.UploadAsync(FilePath, list);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> UpdateItemAsync(string id, T item)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteItemAsync(string id)
    {
        var existingList = await JsonFileHelper.ReloadAsync<T>(FilePath);

        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            var idx = list.FindIndex(x => x.Id == id);
            if (idx != -1)
            {
                list.RemoveAt(idx);
                await JsonFileHelper.UploadAsync(FilePath, list);
                return true;
            }
        }
        return false;
    }

    public virtual async Task DeleteAllAsync()
    {
        await JsonFileHelper.UploadAsync(FilePath, new List<T>());
    }
}
