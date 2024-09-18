# JsonAsDataStorage

```csharp
public class User
{
  public string UserId { get; set; }
  public string UserName { get; set; }
}

// init
IBaseStorage<User> userStorage = new BaseStorage<User>(filePath: "users.json", idField: "UserId");

var user = new User
{
  UserId = Guid.NewGuid().ToString(),
  UserName = "Kirill"
};

// insert new data
await userStorage.InsertItemAsync(user);

// get by id
var result = await userStorage.GetItemAsync(user.UserId);

// update by id
await userStorage.UpdateItemAsync(user.UserId, new User { UserId = user.UserId, UserName = "Alexey" });

// delete by id
await userStorage.DeleteItemAsync(user.UserId);

```
