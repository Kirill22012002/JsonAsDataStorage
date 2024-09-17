namespace JsonAsDataStorage.Core;

public interface IBaseStorage<T> where T : BaseModel
{
    Task<T> GetItemAsync(string id);
    Task<IEnumerable<T>> GetAllItemsAsync();
    Task<bool> InsertItemAsync(T item);
    Task<bool> ReplaceItemAsync(string id, T item);
    Task<bool> UpdateItemAsync(string keidy, T item);
    Task<bool> DeleteItemAsync(string id);
}
