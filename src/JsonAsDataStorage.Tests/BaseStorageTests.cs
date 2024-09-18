using JsonAsDataStorage.Core;

namespace JsonAsDataStorage.Tests;

public class BaseStorageTests
{
    private readonly IBaseStorage<User> _userStorage;
    private readonly IBaseStorage<Car> _carStorage;

    public BaseStorageTests()
    {
        _userStorage = new BaseStorage<User>(filePath: "testUsers.json", idField: "Id");
        _carStorage = new BaseStorage<Car>(filePath: "testCars.json", idField: "CarId");
    }

    #region User 

    [Fact]
    public async Task InsertItemAsync_User()
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            Name = "test1",
            Age = 18
        };

        var result = await _userStorage.InsertItemAsync(user);
        var userJson = await _userStorage.GetItemAsync(user.Id);

        Assert.True(result);
        Assert.NotNull(userJson);
        Assert.Equal(user.Id, userJson.Id);
        Assert.Equal(user.Name, userJson.Name);
        Assert.Equal(user.Age, userJson.Age);
    }

    [Fact]
    public async Task ReplaceItemAsync_User()
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            Name = "test4",
            Age = 19
        };

        // Insert
        var result = await _userStorage.InsertItemAsync(user);
        var userJson = await _userStorage.GetItemAsync(user.Id);

        Assert.True(result);
        Assert.NotNull(userJson);
        Assert.Equal(user.Id, userJson.Id);
        Assert.Equal(user.Name, userJson.Name);
        Assert.Equal(user.Age, userJson.Age);

        // Replace
        var updatedUser = new User
        {
            Id = id,
            Name = "test4.1",
            Age = 22
        };

        var result2 = await _userStorage.ReplaceItemAsync(id, updatedUser);
        var userJson2 = await _userStorage.GetItemAsync(user.Id);

        Assert.True(result2);
        Assert.NotNull(userJson2);
        Assert.Equal(updatedUser.Id, userJson2.Id);
        Assert.Equal(updatedUser.Name, userJson2.Name);
        Assert.Equal(updatedUser.Age, userJson2.Age);

        // Delete 
        var result3 = await _userStorage.DeleteItemAsync(id);
        var allUsers = await _userStorage.GetAllItemsAsync();
        var deletedUser = allUsers.FirstOrDefault(x => x.Id == id);

        Assert.True(result3);
        Assert.Null(deletedUser);

        // Replace
        var updatedUser2 = new User
        {
            Id = id,
            Name = "test4.2",
            Age = 23
        };

        var result4 = await _userStorage.ReplaceItemAsync(id, updatedUser2);
        var userJson4 = await _userStorage.GetItemAsync(user.Id);

        Assert.True(result4);
        Assert.NotNull(userJson4);
        Assert.Equal(updatedUser2.Id, userJson4.Id);
        Assert.Equal(updatedUser2.Name, userJson4.Name);
        Assert.Equal(updatedUser2.Age, userJson4.Age);
    }

    [Fact]
    public async Task UpdateItemAsync_User()
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            Name = "test2",
            Age = 19
        };

        // Insert
        var result = await _userStorage.InsertItemAsync(user);
        var userJson = await _userStorage.GetItemAsync(user.Id);

        Assert.True(result);
        Assert.NotNull(userJson);
        Assert.Equal(user.Id, userJson.Id);
        Assert.Equal(user.Name, userJson.Name);
        Assert.Equal(user.Age, userJson.Age);

        // Update
        var updatedUser = new User
        {
            Id = id,
            Name = "test2.1",
            Age = 20
        };

        var result2 = await _userStorage.UpdateItemAsync(id, updatedUser);
        var userJson2 = await _userStorage.GetItemAsync(user.Id);

        Assert.True(result2);
        Assert.NotNull(userJson2);
        Assert.Equal(updatedUser.Id, userJson2.Id);
        Assert.Equal(updatedUser.Name, userJson2.Name);
        Assert.Equal(updatedUser.Age, userJson2.Age);
    }

    [Fact]
    public async Task DeleteItemAsync_User()
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            Name = "test3",
            Age = 21
        };

        // Insert
        var result = await _userStorage.InsertItemAsync(user);
        var userJson = await _userStorage.GetItemAsync(user.Id);

        Assert.True(result);
        Assert.NotNull(userJson);
        Assert.Equal(user.Id, userJson.Id);
        Assert.Equal(user.Name, userJson.Name);
        Assert.Equal(user.Age, userJson.Age);

        // Delete
        var result2 = await _userStorage.DeleteItemAsync(id);
        var allUsers = await _userStorage.GetAllItemsAsync();
        var deletedUser = allUsers.FirstOrDefault(x => x.Id == id);

        Assert.True(result2);
        Assert.Null(deletedUser);
    }

    #endregion

    #region Car

    [Fact]
    public async Task InsertItemAsync_Car()
    {
        var id = Guid.NewGuid().ToString();
        var car = new Car
        {
            CarId = id,
            CarName = "test1",
            CarMotour = new Motour
            {
                Name = "test-1"
            }
        };

        var result = await _carStorage.InsertItemAsync(car);
        var carJson = await _carStorage.GetItemAsync(car.CarId);

        Assert.True(result);
        Assert.NotNull(carJson);
        Assert.Equal(car.CarId, carJson.CarId);
        Assert.Equal(car.CarName, carJson.CarName);
        Assert.Equal(car.CarMotour.Name, carJson.CarMotour.Name);
    }

    [Fact]
    public async Task ReplaceItemAsync_Car()
    {
        var id = Guid.NewGuid().ToString();
        var car = new Car
        {
            CarId = id,
            CarName = "test6",
            CarMotour = new Motour
            {
                Name = "test-6"
            }
        };

        // Insert
        var result = await _carStorage.InsertItemAsync(car);
        var carJson = await _carStorage.GetItemAsync(car.CarId);

        Assert.True(result);
        Assert.NotNull(carJson);
        Assert.Equal(car.CarId, carJson.CarId);
        Assert.Equal(car.CarName, carJson.CarName);
        Assert.Equal(car.CarMotour.Name, carJson.CarMotour.Name);

        // Replace
        var updatedCar = new Car
        {
            CarId = id,
            CarName = "test7",
            CarMotour = new Motour
            {
                Name = "test-7"
            }
        };

        var result2 = await _carStorage.ReplaceItemAsync(id, updatedCar);
        var carJson2 = await _carStorage.GetItemAsync(car.CarId);

        Assert.True(result2);
        Assert.NotNull(carJson2);
        Assert.Equal(updatedCar.CarId, carJson2.CarId);
        Assert.Equal(updatedCar.CarName, carJson2.CarName);
        Assert.Equal(updatedCar.CarMotour.Name, carJson2.CarMotour.Name);

        // Delete 
        var result3 = await _carStorage.DeleteItemAsync(id);
        var allCars = await _carStorage.GetAllItemsAsync();
        var deletedCar = allCars.FirstOrDefault(x => x.CarId == id);

        Assert.True(result3);
        Assert.Null(deletedCar);

        // Replace
        var updatedCar2 = new Car
        {
            CarId = id,
            CarName = "test8",
            CarMotour = new Motour
            {
                Name = "test-8"
            }
        };

        var result4 = await _carStorage.ReplaceItemAsync(id, updatedCar2);
        var carJson4 = await _carStorage.GetItemAsync(car.CarId);

        Assert.True(result4);
        Assert.NotNull(carJson4);
        Assert.Equal(updatedCar2.CarId, carJson4.CarId);
        Assert.Equal(updatedCar2.CarName, carJson4.CarName);
        Assert.Equal(updatedCar2.CarMotour.Name, carJson4.CarMotour.Name);
    }

    [Fact]
    public async Task UpdateItemAsync_Car()
    {
        var id = Guid.NewGuid().ToString();
        var car = new Car
        {
            CarId = id,
            CarName = "test2",
            CarMotour = new Motour
            {
                Name = "test-2"
            }
        };

        // Insert
        var result = await _carStorage.InsertItemAsync(car);
        var carJson = await _carStorage.GetItemAsync(car.CarId);

        Assert.True(result);
        Assert.NotNull(carJson);
        Assert.Equal(car.CarId, carJson.CarId);
        Assert.Equal(car.CarName, carJson.CarName);
        Assert.Equal(car.CarMotour.Name, carJson.CarMotour.Name);

        // Update
        var updatedCar = new Car
        {
            CarId = id,
            CarName = "test2.1",
            CarMotour = new Motour
            {
                Name = "test-2.1"
            }
        };

        var result2 = await _carStorage.UpdateItemAsync(id, updatedCar);
        var carJson2 = await _carStorage.GetItemAsync(car.CarId);

        Assert.True(result2);
        Assert.NotNull(carJson2);
        Assert.Equal(updatedCar.CarId, carJson2.CarId);
        Assert.Equal(updatedCar.CarName, carJson2.CarName);
        Assert.Equal(updatedCar.CarMotour.Name, carJson2.CarMotour.Name);
    }

    [Fact]
    public async Task DeleteItemAsync_Car()
    {
        var id = Guid.NewGuid().ToString();
        var car = new Car
        {
            CarId = id,
            CarName = "test3",
            CarMotour = new Motour
            {
                Name = "test-3"
            }
        };

        // Insert
        var result = await _carStorage.InsertItemAsync(car);
        var carJson = await _carStorage.GetItemAsync(car.CarId);

        Assert.True(result);
        Assert.NotNull(carJson);
        Assert.Equal(car.CarId, carJson.CarId);
        Assert.Equal(car.CarName, carJson.CarName);
        Assert.Equal(car.CarMotour.Name, carJson.CarMotour.Name);

        // Delete
        var result2 = await _carStorage.DeleteItemAsync(id);
        var allCars = await _carStorage.GetAllItemsAsync();
        var deletedCar = allCars.FirstOrDefault(x => x.CarId == id);

        Assert.True(result2);
        Assert.Null(deletedCar);
    }

    #endregion
}