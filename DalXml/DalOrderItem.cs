namespace Dal;
using DalApi;
using DO;

internal class DalOrderItem : IOrderItem
{
    public int Add(OrderItem t)
    {
        throw new NotImplementedException();
    }

    public void Delete(int n)
    {
        throw new NotImplementedException();
    }

    public OrderItem? Get(Func<OrderItem?, bool> f)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(OrderItem t)
    {
        throw new NotImplementedException();
    }
}
