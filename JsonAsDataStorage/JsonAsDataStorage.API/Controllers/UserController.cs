using Microsoft.AspNetCore.Mvc;

namespace JsonAsDataStorage.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    public UserController()
    {
    }

    [HttpGet]
    public IActionResult AddNewUser([FromBody] UserDto dto)
    {
        var entity = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name,
        };

        // storage.InsertItem(entity);

        return Ok();
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

