namespace JsonAsDataStorage.Core;

public interface IBaseStorage<T>
{
    Task<T> GetItemAsync(dynamic id);
    Task<IEnumerable<T>> GetAllItemsAsync();
    Task<bool> InsertItemAsync(T item);
    Task<bool> ReplaceItemAsync(dynamic id, T item);
    Task<bool> UpdateItemAsync(dynamic id, T item);
    Task<bool> DeleteItemAsync(dynamic id);
}
