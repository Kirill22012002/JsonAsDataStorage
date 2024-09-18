namespace JsonAsDataStorage.Core;

public interface IBaseStorage<T>
{
    Task<T> GetItemAsync(dynamic id);
    Task<T> GetItemAsync(Predicate<T> filter);
    Task<IEnumerable<T>> GetAllItemsAsync();
    Task<bool> InsertItemAsync(T item);
    Task<bool> InsertListAsync(IEnumerable<T> items);
    Task<bool> ReplaceItemAsync(dynamic id, T item);
    Task<bool> ReplaceItemAsync(Predicate<T> filter, T item);
    Task<bool> UpdateItemAsync(dynamic id, T item);
    Task<bool> UpdateItemAsync(Predicate<T> filter, T item);
    Task<bool> DeleteItemAsync(dynamic id);
    Task<bool> DeleteItemAsync(Predicate<T> filter);
    Task DeleteAllAsync();
}
