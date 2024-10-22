namespace JsonAsDataStorage.Core;

public class DirectoryStorage : BaseStorage<DirectoryItem>
{
    public DirectoryStorage(string filePath, string idField) : base(filePath, idField) { }

    public override async Task<bool> InsertItemAsync(DirectoryItem item)
    {
        if (item?.ParentId == 0)
        {
            return await base.InsertItemAsync(item);
        }
        else
        {
            var existingList = await JsonFileHelper.ReloadAsync<DirectoryItem>(_filePath);
            var list = existingList.ToList();

            var necessaryItem = RecursiveGetById(item.ParentId, list);

            if (necessaryItem == null) return false;
            necessaryItem.SubDirectories.Add(item);
            await JsonFileHelper.UploadAsync(_filePath, list);
            return true;
        }
    }

    public async Task<DirectoryItem> GetItemAsync(int id)
    {
        var existingList = await JsonFileHelper.ReloadAsync<DirectoryItem>(_filePath);
        if (existingList != null)
        {
            var item = RecursiveGetById(id, existingList.ToList());
            if (item != null)
            {
                return item;
            }
        }
        throw new KeyNotFoundException($"Item not found");
    }

    private DirectoryItem RecursiveGetById(int id, List<DirectoryItem> items)
    {
        DirectoryItem result = null;
        foreach (var item in items)
        {
            if (item.Id == id)
            {
                result = item;
                return item;
            }
            else
            {
                var dirItem = RecursiveGetById(id, item.SubDirectories);
                if (dirItem == null) continue;
                else return dirItem;
            }
        }
        return result;
    }
}

public class DirectoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ParentId { get; set; }
    public List<DirectoryItem> SubDirectories { get; set; } = new List<DirectoryItem>();
    public List<FileItem> Files { get; set; } = new List<FileItem>();
}

public class FileItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}
