using JsonAsDataStorage.Core;
using Microsoft.AspNetCore.Mvc;

namespace JsonAsDataStorage.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DirectoryItemController : ControllerBase
{
    private readonly IDirectoryStorage _storage;

    public DirectoryItemController()
    {
        _storage = new DirectoryStorage(filePath: "directories.json", idField: "Id");
    }

    [HttpPost]
    public async Task<IActionResult> AddDirectory([FromBody] AddDirectoryDto dto)
    {
        var entity = new DirectoryItem
        {
            Id = new Random().Next(1, int.MaxValue),
            ParentId = dto.ParentId,
            Name = dto.Name,
        };

        var result = await _storage.InsertItemAsync(entity);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateDirectory([FromBody] UpdateDirectoryDto dto)
    {
        var entity = new DirectoryItem
        {
            Id = dto.Id,
            Name = dto.Name
        };

        var result = await _storage.UpdateItemAsync(entity, entity.Id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> RemoveDirectory([FromQuery] int id)
    {
        var result = await _storage.DeleteItemAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetDirectory([FromQuery] int id)
    {
        var result = await _storage.GetItemAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDirectories()
    {
        var result = await _storage.GetAllItemsAsync();
        return Ok(result);
    }
}

public class AddDirectoryDto
{
    public int ParentId { get; set; }
    public string Name { get; set; }
}

public class UpdateDirectoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}
