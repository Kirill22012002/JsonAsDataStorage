namespace JsonAsDataStorage.Core;

public interface IBaseStorage<T> where T : BaseModel
{
    T GetItem(string id);
    IEnumerable<T> GetAllItems();
    bool InsertItem(T item);
    bool ReplaceItem(string id, T item);
    bool UpdateItem(string keidy, T item);
    bool DeleteItem(string id);
}
