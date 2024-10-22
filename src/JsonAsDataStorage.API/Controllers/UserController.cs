using JsonAsDataStorage.Core;
using Microsoft.AspNetCore.Mvc;

namespace JsonAsDataStorage.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IBaseStorage<User> _storage;

    public UserController()
    {
        _storage = new BaseStorage<User>(filePath: "users.json", idField: "Id");
    }

    [HttpGet]
    public async Task<IActionResult> GetUserById([FromQuery] string id)
    {
        var result = await _storage.GetItemAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _storage.GetAllItemsAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewUser([FromBody] UserDto dto)
    {
        var entity = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name,
        };
        var result = await _storage.InsertItemAsync(entity);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateUser([FromQuery] string id, [FromQuery] string name)
    {
        var entity = new User { Id = id, Name = name };
        var result = await _storage.ReplaceItemAsync(id, entity);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteUser([FromQuery] string id)
    {
        var result = await _storage.DeleteItemAsync(id);
        return Ok(result);
    }
}

public class UserDto
{
    public string Name { get; set; } = "No name";
}


public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
}

