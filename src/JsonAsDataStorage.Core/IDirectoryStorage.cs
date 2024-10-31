namespace JsonAsDataStorage.Core;

public interface IDirectoryStorage
{
    Task<DirectoryItem> GetItemAsync(int id);
    Task<IEnumerable<DirectoryItem>> GetAllItemsAsync();
    Task<bool> InsertItemAsync(DirectoryItem item);
    Task<bool> UpdateItemAsync(DirectoryItem item, int id);
    Task<bool> DeleteItemAsync(int id);
}
