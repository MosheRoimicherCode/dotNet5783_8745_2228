using BO;
namespace BlApi
{
    public interface IBoOrder
    {
        /// return a list with all orders
        /// <returns> order list </returns>
        public List<BoOrderForList> GetLists();

        ///search for a order with specific Id 
        /// <returns> IBoOrder item </returns>
        public BoOrder Get(int Id);

        ///search for a order that has not shipped yet with specific Id 
        ///update shipping date, and returns updated order
        public BoOrder UpdateShipping(int Id);

        ///search for a order that has shipped but has not provided yet with specific Id 
        ///update providing date, and returns updated order
        public BoOrder UpdateProviding(int Id);

        ///search for a order with specific Id 
        ///returns OrderTracking of this order
        public BoOrderTracking OrderTracking(int Id);

        ///update order by the manager
        public void UpdateOrder(BoOrder item);
    }
} /// interface of order items for manager and client
