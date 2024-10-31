using JsonAsDataStorage.Core;
using Newtonsoft.Json;
using System.Reflection;

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
            Id = GetRandomId(),
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

    #region Recursive functions 

    [Fact]
    public void RecursiveGetById_ShouldReturnCorrectItem()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id15,
                Name = "D:"
            }
        };
        DirectoryItem expectedItem = new DirectoryItem
        {
            Id = id5,
            Name = "IIS",
            SubDirectories = new List<DirectoryItem>
            {
                new DirectoryItem
                {
                    Id = id,
                    Name = "Asp.Net Core Module",
                    SubDirectories = new List<DirectoryItem>
                    {
                        new DirectoryItem
                        {
                            Id = id6,
                            Name = "V2",
                            Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                            SubDirectories = new List<DirectoryItem>
                            {
                                new DirectoryItem
                                {
                                    Id = id8,
                                    Name = "18.0.24115",
                                    Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                }
                            }
                        }
                    }
                },
                new DirectoryItem
                {
                    Id = id10,
                    Name = "Microsoft Web Deploy",
                    Files = new List<FileItem>
                    {
                        new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                        new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                    },
                },
                new DirectoryItem
                {
                    Id = id13,
                    Name = "Microsoft Web Deploy V3"
                }
             }
        };

        //Act
        var result = sourceList.RecursiveGetById(id5);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedItem);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecursiveGetById_ShouldReturnCorrectItem_2()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id15,
                Name = "D:"
            }
        };
        DirectoryItem expectedItem = new DirectoryItem
        {
            Id = id15,
            Name = "D:"
        };

        //Act
        var result = sourceList.RecursiveGetById(id15);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedItem);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecursiveGetById_ShouldReturnCorrectItem_3()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id15,
                Name = "D:"
            }
        };
        DirectoryItem expectedItem = new DirectoryItem
        {
            Id = id8,
            Name = "18.0.24115",
            Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
        };

        //Act
        var result = sourceList.RecursiveGetById(id8);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedItem);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecursiveGetById_ShouldReturnCorrectItem_4()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id15,
                Name = "D:"
            }
        };
        DirectoryItem expectedItem = new DirectoryItem
        {
            Id = id,
            Name = "Asp.Net Core Module",
            SubDirectories = new List<DirectoryItem>
            {
                new DirectoryItem
                {
                    Id = id6,
                    Name = "V2",
                    Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                    SubDirectories = new List<DirectoryItem>
                    {
                        new DirectoryItem
                        {
                            Id = id8,
                            Name = "18.0.24115",
                            Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                        }
                    }
                }
            }
        };

        //Act
        var result = sourceList.RecursiveGetById(id);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedItem);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecursiveDelete_ShouldReturnCorrectItems()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id15,
                Name = "D:"
            }
        };
        List<DirectoryItem> expectedList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id15,
                Name = "D:"
            }
        };

        //Act
        var result = sourceList.RecursiveDelete(id);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedList);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecursiveDelete_ShouldReturnCorrectItems_2()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id15,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id,
                Name = "D:"
            }
        };
        List<DirectoryItem> expectedList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id4,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id15,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            }
        };

        //Act
        var result = sourceList.RecursiveDelete(id);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedList);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecursiveDelete_ShouldReturnCorrectItems_3()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id15,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id8,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id4,
                Name = "D:"
            }
        };
        List<DirectoryItem> expectedList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id4,
                Name = "D:"
            }
        };

        //Act
        var result = sourceList.RecursiveDelete(id);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedList);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RecursiveDelete_ShouldReturnCorrectItems_4()
    {
        // Arrange
        int id = GetRandomId();
        int id1 = GetRandomId();
        int id2 = GetRandomId();
        int id3 = GetRandomId();
        int id4 = GetRandomId();
        int id5 = GetRandomId();
        int id6 = GetRandomId();
        int id7 = GetRandomId();
        int id8 = GetRandomId();
        int id9 = GetRandomId();
        int id10 = GetRandomId();
        int id11 = GetRandomId();
        int id12 = GetRandomId();
        int id13 = GetRandomId();
        int id14 = GetRandomId();
        int id15 = GetRandomId();

        List<DirectoryItem> sourceList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id8,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id15,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } },
                                                SubDirectories = new List<DirectoryItem>
                                                {
                                                    new DirectoryItem
                                                    {
                                                        Id = id,
                                                        Name = "18.0.24115",
                                                        Files = new List<FileItem> { new FileItem { Id = id9, Name = "aspnetcorev2_outofprocess.dll" } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id4,
                Name = "D:"
            }
        };
        List<DirectoryItem> expectedList = new List<DirectoryItem>
        {
            new DirectoryItem
            {
                Id = id1,
                Name = "C:",
                Files = new List<FileItem> { new FileItem { Id = id2, Name = "DumpStack.log" }, new FileItem { Id = id3, Name = "21092004.log" } },
                SubDirectories = new List<DirectoryItem>
                {
                    new DirectoryItem
                    {
                        Id = id8,
                        Name = "Program Files",
                        SubDirectories = new List<DirectoryItem>
                        {
                            new DirectoryItem
                            {
                                Id = id5,
                                Name = "IIS",
                                SubDirectories = new List<DirectoryItem>
                                {
                                    new DirectoryItem
                                    {
                                        Id = id15,
                                        Name = "Asp.Net Core Module",
                                        SubDirectories = new List<DirectoryItem>
                                        {
                                            new DirectoryItem
                                            {
                                                Id = id6,
                                                Name = "V2",
                                                Files = new List<FileItem> { new FileItem { Id = id7, Name = "aspnetcorev2.dll" } }
                                            }
                                        }
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id10,
                                        Name = "Microsoft Web Deploy",
                                        Files = new List<FileItem>
                                        {
                                            new FileItem { Id = id11, Name = "Microsoft.Web.Deployment.dll" },
                                            new FileItem { Id = id12, Name = "Microsoft.Web.Deployment.Tracing.dll" },
                                        },
                                    },
                                    new DirectoryItem
                                    {
                                        Id = id13,
                                        Name = "Microsoft Web Deploy V3"
                                    }
                                }
                            }
                        }
                    },
                    new DirectoryItem
                    {
                        Id = id14,
                        Name = "Program Files (x86)",
                    }
                }
            },
            new DirectoryItem
            {
                Id = id4,
                Name = "D:"
            }
        };

        //Act
        var result = sourceList.RecursiveDelete(id);

        //Assert 
        var expected = JsonConvert.SerializeObject(expectedList);
        var actual = JsonConvert.SerializeObject(result);
        Assert.Equal(expected, actual);
    }

    #endregion

    private int GetRandomId()
    {
        return _random.Next(1, int.MaxValue);
    }
}