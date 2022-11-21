using DO;
namespace DalApi;

public interface IOrder : ICrud<Order>
{
    public List<Order> CopyOrderList();
}
