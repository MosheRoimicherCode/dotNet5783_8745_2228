using DO;
namespace DalApi;

public interface IOrderItem : ICrud<OrderItem>
{
    public List<OrderItem> CopyOrderItemArray();
}
