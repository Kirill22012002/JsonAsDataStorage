using JsonAsDataStorage.Core;
using Microsoft.AspNetCore.Mvc;

namespace JsonAsDataStorage.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DirectoryItemController : ControllerBase
{
    private readonly DirectoryStorage _storage;

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
}

public class AddDirectoryDto
{
    public int ParentId { get; set; }
    public string Name { get; set; }
}
