public interface ICrud<T>
{
    void add(T t);
    void delete(T t);
    void update();
    T get();  
}