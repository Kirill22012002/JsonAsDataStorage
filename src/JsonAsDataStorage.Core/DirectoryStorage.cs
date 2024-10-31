namespace JsonAsDataStorage.Core;

public class DirectoryStorage : BaseStorage<DirectoryItem>, IDirectoryStorage
{
    public DirectoryStorage(string filePath, string idField) : base(filePath, idField) { }

    public async Task<DirectoryItem> GetItemAsync(int id)
    {
        var existingList = await JsonFileHelper.ReloadAsync<DirectoryItem>(_filePath);
        if (existingList != null)
        {
            var list = existingList.ToList();
            var item = list.RecursiveGetById(id);
            if (item != null)
            {
                return item;
            }
        }
        throw new KeyNotFoundException($"Item not found");
    }

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

            var necessaryItem = list.RecursiveGetById(item.ParentId);

            if (necessaryItem == null) return false;
            necessaryItem.SubDirectories.Add(item);
            await JsonFileHelper.UploadAsync(_filePath, list);
            return true;
        }
    }

    public async Task<bool> UpdateItemAsync(DirectoryItem item, int id)
    {
        var existingList = await JsonFileHelper.ReloadAsync<DirectoryItem>(_filePath);
        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            var necessaryItem = list.RecursiveGetById(item.Id);
            if (necessaryItem == null) return false;

            necessaryItem.Name = item.Name;

            await JsonFileHelper.UploadAsync(_filePath, list);
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var existingList = await JsonFileHelper.ReloadAsync<DirectoryItem>(_filePath);
        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            list = list.RecursiveDelete(id);
            await JsonFileHelper.UploadAsync(_filePath, list);

            return true;
        }
        return false;
    }
}

public static class RecusriveDirectoryItem
{
    public static DirectoryItem RecursiveGetById(this List<DirectoryItem> items, int id)
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
                var dirItem = item.SubDirectories.RecursiveGetById(id);
                if (dirItem == null) continue;
                else return dirItem;
            }
        }
        return result;
    }

    public static List<DirectoryItem> RecursiveDelete(this List<DirectoryItem> sourceList, int id)
    {
        foreach (var item in sourceList)
        {
            if (item.Id == id)
            {
                sourceList.Remove(item);
                return sourceList;
            }
            else
            {
                item.SubDirectories = item.SubDirectories.RecursiveDelete(id);
            }
        }
        return sourceList;
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
