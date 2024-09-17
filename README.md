# JsonAsDataStorage

public class BaseModel 
{
  public string Id { get; set; }
}

public class BaseStorage<T> where T: BaseModel 
{

}
