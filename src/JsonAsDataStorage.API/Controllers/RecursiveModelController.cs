using JsonAsDataStorage.Core;
using Microsoft.AspNetCore.Mvc;

namespace JsonAsDataStorage.API.Controllers;

public class RecursiveModelDto
{
    public string Id { get; set; }
    public string Code { get; set; }
    public int Steps { get; set; }
    public List<RecursiveModelDto> ChildModels { get; set; }
}

public class RecursiveModel
{
    public string Id { get; set; }
    public string Code { get; set; }
    public int Steps { get; set; }
    public List<RecursiveModel> ChildModels { get; set; }
}

[ApiController]
[Route("[controller]/[action]")]
public class RecursiveModelController : ControllerBase
{
    private readonly IBaseStorage<RecursiveModel> _storage;

    public RecursiveModelController()
    {
        _storage = new BaseStorage<RecursiveModel>(filePath: "recursiveModels.json", idField: "Id");
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
        var result = await _storage.GetItemAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _storage.GetAllItemsAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddNew()
    {
        var entity = new RecursiveModel
        {
            Id = Guid.NewGuid().ToString(),
            Code = "375",
            Steps = 0,
            ChildModels = new List<RecursiveModel> 
            { 
                new RecursiveModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "29",
                    Steps = 1,
                    ChildModels = new List<RecursiveModel>
                    {
                        new RecursiveModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            Code = "601",
                            Steps = 2,
                            ChildModels = new List<RecursiveModel>
                            {
                                new RecursiveModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Code = "44",
                                    Steps = 3,
                                    ChildModels = new List<RecursiveModel>
                                    {
                                        new RecursiveModel
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Code = "44",
                                            Steps = 4
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new RecursiveModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "33",
                    Steps = 1
                }
            }
        };
        var result = await _storage.InsertItemAsync(entity);
        
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
        var result = await _storage.DeleteItemAsync(id);
        return Ok(result);
    }
}
