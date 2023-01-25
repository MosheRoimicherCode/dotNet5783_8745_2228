namespace DalApi;
using DO;
public interface IOrderItem : ICrud<OrderItem>
{
    public void DeleteProduct(int productId);
}

