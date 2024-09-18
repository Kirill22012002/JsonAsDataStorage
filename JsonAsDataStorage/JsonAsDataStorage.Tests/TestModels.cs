namespace JsonAsDataStorage.Tests;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public class Car
{
    public string CarId { get; set; }
    public string CarName { get; set; }
    public Motour CarMotour { get; set; }
}

public class Motour
{
    public string Name { get; set; }
}