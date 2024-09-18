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



```
