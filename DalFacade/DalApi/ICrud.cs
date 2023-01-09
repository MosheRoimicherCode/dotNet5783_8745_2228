using DO;

public interface ICrud<T> where T : struct
{
    int Add(T t);
    void Delete(int n);
    void Update(T t);
    T? Get(Func<T?, bool> f);
    IEnumerable<T?> GetAll(Func<T?, bool>? filter = null);
}

