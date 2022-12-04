using DO;

public interface ICrud<T>
{
    int Add(T t);
    void Delete(int n);
    void Update(int n, T t);
    T Get(int n);
    T Get(int n, Func<T?, bool> f);
    void GetAll(Func<T?, bool>? f);
    List<T> CopyList();
}

