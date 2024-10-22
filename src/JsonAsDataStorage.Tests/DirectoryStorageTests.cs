using JsonAsDataStorage.Core;
using Newtonsoft.Json;

namespace JsonAsDataStorage.Tests;

public class DirectoryStorageTests
{
    private readonly DirectoryStorage _storage;

    private Random _random = new Random();

    public DirectoryStorageTests()
    {
        _storage = new DirectoryStorage(filePath: "testDirectories.json", idField: "Id");
    }

    [Fact]
    public async Task InsertItemAsync_ShouldReturnTrue()
    {
        // Arrange
        var item = new DirectoryItem
        {
            Id = _random.Next(1, int.MaxValue),
            Name = "C:"
        };

        // Act
        var result = await _storage.InsertItemAsync(item);
        var resultJson = await _storage.GetItemAsync(item.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(resultJson);

        var expected = JsonConvert.SerializeObject(item);
        var actual = JsonConvert.SerializeObject(resultJson);
        Assert.Equal(expected, actual);

    }

    [Fact]
    public async Task InsertItemAsync_ShouldReturnTrue_2()
    {
        // Arrange
        var item = new DirectoryItem
        {
            Id = GetRandomId(),
            Name = "C:"
        };

        item.Files.Add(new FileItem { Id = GetRandomId(), Name = "DumpStack.log" });

        item.SubDirectories.Add(new DirectoryItem { Id = GetRandomId(), Name = "Program Files", ParentId = item.Id });
        item.SubDirectories.Add(new DirectoryItem { Id = GetRandomId(), Name = "Program Files (x86)", ParentId = item.Id });

        // Act
        var result = await _storage.InsertItemAsync(item);
        var resultJson = await _storage.GetItemAsync(item.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(resultJson);

        var expected = JsonConvert.SerializeObject(item);
        var actual = JsonConvert.SerializeObject(resultJson);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InsertItemAsync_ShouldReturnTrue_3()
    {
        // Arrange
        var cId = GetRandomId();
        var pFId = GetRandomId();
        var iisId = GetRandomId();
        var ancmId = GetRandomId();
        var v2Id = GetRandomId();
        var numbersId = GetRandomId();
        var mWDId = GetRandomId();
        var mWDV3Id = GetRandomId();
        var item = new DirectoryItem
        {
            Id = cId,
            Name = "C:",
            Files = new List<FileItem> { new FileItem { Id = GetRandomId(), Name = "DumpStack.log" }, new FileItem { Id = GetRandomId(), Name = "21092004.log" } },
            SubDirectories = new List<DirectoryItem>
            {
                new DirectoryItem
                {
                    Id = GetRandomId(),
                    ParentId = cId,
                    Name = "Program Files",
                    SubDirectories = new List<DirectoryItem>
                    {
                        new DirectoryItem
                        {
                            Id = iisId,
                            ParentId = pFId,
                            Name = "IIS",
                            SubDirectories = new List<DirectoryItem>
                            {
                                new DirectoryItem
                                {
                                    Id = ancmId,
                                    ParentId = iisId,
                                    Name = "Asp.Net Core Module",
                                    SubDirectories = new List<DirectoryItem>
                                    {
                                        new DirectoryItem
                                        {
                                            Id = v2Id,
                                            ParentId = ancmId,
                                            Name = "V2",
                                            Files = new List<FileItem> { new FileItem { Id = GetRandomId(), Name = "aspnetcorev2.dll" } },
                                            SubDirectories = new List<DirectoryItem>
                                            {
                                                new DirectoryItem
                                                {
                                                    Id = numbersId,
                                                    ParentId = v2Id,
                                                    Name = "18.0.24115",
                                                    Files = new List<FileItem> { new FileItem { Id = GetRandomId(), Name = "aspnetcorev2_outofprocess.dll" } }
                                                }
                                            }
                                        }
                                    }
                                },
                                new DirectoryItem
                                {
                                    Id = mWDId,
                                    ParentId = iisId,
                                    Name = "Microsoft Web Deploy",
                                    Files = new List<FileItem>
                                    {
                                        new FileItem { Id = GetRandomId(), Name = "Microsoft.Web.Deployment.dll" },
                                        new FileItem { Id = GetRandomId(), Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                    },
                                },
                                new DirectoryItem
                                {
                                    Id = mWDV3Id,
                                    ParentId = iisId,
                                    Name = "Microsoft Web Deploy V3"
                                }
                            }
                        }
                    }
                },
                new DirectoryItem
                {
                    Id = GetRandomId(),
                    ParentId = cId,
                    Name = "Program Files (x86)",
                }
            }
        };

        // Act
        var result = await _storage.InsertItemAsync(item);
        var resultJson = await _storage.GetItemAsync(item.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(resultJson);

        var expected = JsonConvert.SerializeObject(item);
        var actual = JsonConvert.SerializeObject(resultJson);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InsertItemAsyncByParentId_ShouldReturnTrue()
    {
        // Arrange
        var cId = GetRandomId();
        var pFId = GetRandomId();
        var pfx86Id = GetRandomId();
        var iisId = GetRandomId();
        var ancmId = GetRandomId();
        var v2Id = GetRandomId();
        var numbersId = GetRandomId();
        var mWDId = GetRandomId();
        var mWDV3Id = GetRandomId();
        var item = new DirectoryItem
        {
            Id = cId,
            Name = "C:",
            Files = new List<FileItem> { new FileItem { Id = GetRandomId(), Name = "DumpStack.log" }, new FileItem { Id = GetRandomId(), Name = "21092004.log" } },
            SubDirectories = new List<DirectoryItem>
            {
                new DirectoryItem
                {
                    Id = GetRandomId(),
                    ParentId = cId,
                    Name = "Program Files",
                    SubDirectories = new List<DirectoryItem>
                    {
                        new DirectoryItem
                        {
                            Id = iisId,
                            ParentId = pFId,
                            Name = "IIS",
                            SubDirectories = new List<DirectoryItem>
                            {
                                new DirectoryItem
                                {
                                    Id = ancmId,
                                    ParentId = iisId,
                                    Name = "Asp.Net Core Module",
                                    SubDirectories = new List<DirectoryItem>
                                    {
                                        new DirectoryItem
                                        {
                                            Id = v2Id,
                                            ParentId = ancmId,
                                            Name = "V2",
                                            Files = new List<FileItem> { new FileItem { Id = GetRandomId(), Name = "aspnetcorev2.dll" } },
                                            SubDirectories = new List<DirectoryItem>
                                            {
                                                new DirectoryItem
                                                {
                                                    Id = numbersId,
                                                    ParentId = v2Id,
                                                    Name = "18.0.24115",
                                                    Files = new List<FileItem> { new FileItem { Id = GetRandomId(), Name = "aspnetcorev2_outofprocess.dll" } }
                                                }
                                            }
                                        }
                                    }
                                },
                                new DirectoryItem
                                {
                                    Id = mWDId,
                                    ParentId = iisId,
                                    Name = "Microsoft Web Deploy",
                                    Files = new List<FileItem>
                                    {
                                        new FileItem { Id = GetRandomId(), Name = "Microsoft.Web.Deployment.dll" },
                                        new FileItem { Id = GetRandomId(), Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                    },
                                },
                                new DirectoryItem
                                {
                                    Id = mWDV3Id,
                                    ParentId = iisId,
                                    Name = "Microsoft Web Deploy V3"
                                }
                            }
                        }
                    }
                },
                new DirectoryItem
                {
                    Id = pfx86Id,
                    ParentId = cId,
                    Name = "Program Files (x86)",
                }
            }
        };

        var result = await _storage.InsertItemAsync(item);
        var resultJson = await _storage.GetItemAsync(item.Id);

        Assert.True(result);
        Assert.NotNull(resultJson);

        var expected = JsonConvert.SerializeObject(item);
        var actual = JsonConvert.SerializeObject(resultJson);
        Assert.Equal(expected, actual);

        var cfId = GetRandomId();
        var newItem = new DirectoryItem
        {
            Id = cfId,
            ParentId = pfx86Id,
            Name = "Common Files",
            SubDirectories = new List<DirectoryItem>
            {
                new DirectoryItem
                {
                    Id = GetRandomId(),
                    ParentId = cfId,
                    Name = "Adobe"
                }
            }
        };

        // Act 
        var finalResult = await _storage.InsertItemAsync(newItem);
        var finalResultJson = await _storage.GetItemAsync(newItem.Id);

        // Assert
        Assert.True(finalResult);
        Assert.NotNull(finalResultJson);

        var finalExpected = JsonConvert.SerializeObject(newItem);
        var finalActual = JsonConvert.SerializeObject(finalResultJson);
        Assert.Equal(finalExpected, finalActual);

        item.SubDirectories.SingleOrDefault(x => x.Id == pfx86Id).SubDirectories.Add(newItem);

        var finalExpected2 = JsonConvert.SerializeObject(item);
        var finalActual2 = JsonConvert.SerializeObject(await _storage.GetItemAsync(item.Id));
        Assert.Equal(finalExpected2, finalActual2);
    }


    private int GetRandomId()
    {
        return _random.Next(1, int.MaxValue);
    }
}