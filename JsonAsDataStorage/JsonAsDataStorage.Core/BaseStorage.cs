using Newtonsoft.Json;

namespace JsonAsDataStorage.Core;

public class BaseStorage<T> : IBaseStorage<T> where T : BaseModel
{
    public string FilePath { get; set; }

    public virtual T GetItem(string id)
    {
        var existingList = Reload();
        if (existingList != null)
        {
            var list = existingList.ToList();
            var index = list.FindIndex(x => x.Id == id);
            var item = list[index];
            return item;
        }
        throw new Exception();
    }

    public virtual IEnumerable<T> GetAllItems()
    {
        return Reload();
    }

    public virtual bool InsertItem(T item)
    {
        var existingList = Reload();
        if (existingList != null)
        {
            var list = existingList.ToList();
            list.Add(item);
            Upload(list);
            return true;
        }
        else
        {
            Upload(new List<T> { item });
            return true;
        }
    }

    public virtual bool InsertList(IEnumerable<T> items)
    {
        var existingList = Reload();
        if (existingList != null && existingList.Count() != 0)
        {
            var list = existingList.ToList();
            list.AddRange(items);
            Upload(list);
            return true;
        }
        else
        {
            Upload(items);
            return true;
        }
    }

    public bool ReplaceItem(string id, T item)
    {
        var existingList = Reload().ToList();
        if (existingList != null && existingList.Count() != 0)
        {
            var idx = existingList.FindIndex(x => x.Id == id);
            if (idx != -1)
            {
                existingList[idx] = item;
                Upload(existingList);
                return true;
            }
        }
        return false;
    }

    public bool UpdateItem(string id, T item)
    {
        throw new NotImplementedException();
    }

    public bool DeleteItem(string id)
    {
        var existingList = Reload().ToList();
        if (existingList != null && existingList.Count() != 0)
        {
            var idx = existingList.FindIndex(x => x.Id == id);
            if (idx != -1)
            {
                existingList.RemoveAt(idx);
            }
        }
        return false;
    }

    public virtual void DeleteAll()
    {
        Upload(new List<T>());
    }

    // getting data from Json file
    private IEnumerable<T> Reload()
    {
        return JsonConvert.DeserializeObject<IEnumerable<T>>(File.ReadAllText(FilePath));
    }

    // setting data to Json file
    private void Upload(IEnumerable<T> models)
    {
        File.WriteAllText(FilePath, JsonConvert.SerializeObject(models, Formatting.Indented));
    }
}
