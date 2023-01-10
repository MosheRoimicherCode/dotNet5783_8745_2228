using DO;

public interface ICrud<T> where T : struct
{
    int Add(T t);
    void Delete(int ID);
    void Update(T t);
    T? Get(Func<T?, bool> filter);
    IEnumerable<T?> GetAll(Func<T?, bool>? filter = null);
}

