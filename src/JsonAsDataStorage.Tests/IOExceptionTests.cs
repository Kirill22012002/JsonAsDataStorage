using JsonAsDataStorage.Core;

namespace JsonAsDataStorage.Tests;

public class IOExceptionTests
{
    private readonly string _filePath = null;

    public IOExceptionTests()
    {
        _filePath = Path.Combine("cache", "ioExceptionTests.json");
        if (!File.Exists(_filePath))
        {
            new FileInfo(_filePath).Directory.Create();
            using (var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.Read)) { }
        }
    }

    [Fact]
    public async Task ReloadAsync_FileLocked_CorrectDataAfterMultipleReads()
    {
        // Arrange
        var expectedItems = Enumerable.Range(1, 1000).Select(x => new TestItem { Id = x }).ToList();
        await JsonFileHelper.UploadAsync(_filePath, expectedItems);

        // Act
        var tasks = Enumerable.Range(0, 100).Select(async _ => await JsonFileHelper.ReloadAsync<TestItem>(_filePath)).ToList();
        await Task.WhenAll(tasks);

        // Assert
        foreach (var task in tasks)
        {
            var result = task.Result.ToList();
            Assert.Equal(expectedItems.Count, result.Count);
            Assert.True(expectedItems.All(e => result.Any(r => r.Id == e.Id)));
        }
    }

    [Fact]
    public async Task UploadAsync_MultipleWrites_FileContainsCorrectData()
    {
        // Arrange 
        var itemsList = Enumerable.Range(1, 10).Select(i =>
            Enumerable.Range(1, 100).Select(x => new TestItem { Id = x * i }).ToList()).ToList();

        // Act 
        var tasks = itemsList.Select(items => JsonFileHelper.UploadAsync(_filePath, items)).ToList();
        await Task.WhenAll(tasks);

        // Assert
        var resultItems = await JsonFileHelper.ReloadAsync<TestItem>(_filePath);
        Assert.Contains(resultItems, item => itemsList.Any(list => list.Any(l => l.Id == item.Id)));
    }

    [Fact]
    public async Task ReloadAndUpload_StressTest_DataIntegrityMaintained()
    {
        // Arrange
        var initialItems = Enumerable.Range(1, 100).Select(x => new TestItem { Id = x }).ToList();
        await JsonFileHelper.UploadAsync(_filePath, initialItems);

        // Act & Assert
        Parallel.ForEach(Enumerable.Range(0, 20), async i =>
        {
            if (i % 2 == 0)
            {
                var readItems = await JsonFileHelper.ReloadAsync<TestItem>(_filePath);
                Assert.True(initialItems.All(e => readItems.Any(r => r.Id == e.Id)));
            }
            else
            {
                var newItems = Enumerable.Range(1, 100).Select(x => new TestItem { Id = x * i }).ToList();
                var success = await JsonFileHelper.UploadAsync(_filePath, newItems);
                Assert.True(success);
            }
        });
    }
}
