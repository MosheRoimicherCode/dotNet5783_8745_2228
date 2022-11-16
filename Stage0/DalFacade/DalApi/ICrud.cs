public interface ICrud<T>
{
    void Add(T t);
    void Delete(T t);
    void Update();
    T Get();  
}

