using BO;
namespace BlApi
{
    public interface IOrder
    {
        /// return a list with all orders
        /// <returns> order list </returns>
        public IEnumerable<OrderForList> GetList();

        ///search for a order with specific Id 
        /// <returns> IBoOrder item </returns>
        public Order Get(int Id);

        ///search for a order that has not shipped yet with specific Id 
        ///update shipping date, and returns updated order
        public Order UpdateShipping(int Id);

        ///search for a order that has shipped but has not provided yet with specific Id 
        ///update providing date, and returns updated order
        public Order UpdateProviding(int Id);

        ///search for a order with specific Id 
        ///returns OrderTracking of this order
        public OrderTracking OrderTracking(int Id);

        ///update order by the manager
        //public void UpdateOrder(BoOrder item);

        public IEnumerable<BO.OrderTracking> GetListOfTruckings();
        //get last managed order id
        public int? ReturnOrderForManage();
        public BO.Order UpdateStatus(int id);
        public void Delete(int id);
    }
} /// interface of order items for manager and client
