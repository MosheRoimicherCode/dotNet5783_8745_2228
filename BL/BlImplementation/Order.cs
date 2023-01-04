namespace BlImplementation; 

using BO;
using DalApi;
using DO;
using System.Linq;

internal class Order : BlApi.IOrder
{
    IDal? Dal = DalApi.Factory.Get();
    private double price2;
    private int a;
    private double b;

    public double TPrice { get; private set; }

    ///checking the status of the order, returns Enum-status type
    public BO.Enums.Status CheckStatus(DO.Order? o)
    {
        if (o?.ShipDate > DateTime.Now)          return BO.Enums.Status.approved;
        else if (o?.DeliveryDate > DateTime.Now) return BO.Enums.Status.shiped;
        else if (DateTime.Now > o?.DeliveryDate) return BO.Enums.Status.provided;
        else                                     return BO.Enums.Status.error;
    }
    public BO.Order ConvertOrderToBoOrder(DO.Order o)
    {
        BO.Order bo = new BO.Order();
        bo.ShipDate = o.ShipDate;
        bo.DeliveryDate = o.DeliveryDate;
        bo.OrderDate = o.OrderDate;
        bo.CustomerName = o.CustomerName;
        bo.CustomerAdress = o.CustomeAdress;
        bo.CustomerEmail = o.CustomerEmail;
        bo.OrderStatus = CheckStatus(o);
        return bo;
    }
    public BO.Order ConvertDoOrderToBoOrder(int Id)
    {
        DO.Order? dalOrder = Dal?.Order.Get(x => x?.ID == Id)!;
        BO.Order boOrder = new BO.Order();
        
        boOrder.ID = dalOrder.Value.ID;
        boOrder.CustomerName = dalOrder?.CustomerName;
        boOrder.CustomerEmail = dalOrder?.CustomerEmail;
        boOrder.CustomerAdress = dalOrder?.CustomeAdress;
        boOrder.OrderStatus = CheckStatus(dalOrder);
        boOrder.OrderDate = dalOrder?.OrderDate;
        boOrder.ShipDate = dalOrder?.ShipDate;
        boOrder.DeliveryDate = dalOrder?.DeliveryDate;
        boOrder.Details = new();
        boOrder.TotalPrice = 0;

        var boOrderDetailsTuple = (from orderItem in Dal!.OrderItem.GetAll(x => x.Value.OrderID == Id)
                                   let TotalPrice = boOrder.TotalPrice + Dal.Product.Get(x => x.Value.ID == orderItem.Value.ProductID)!.Value.Price
                                   select (TotalPrice, new List<BO.OrderItem>
                                     (
                                         from orderItem in Dal!.OrderItem.GetAll(x => x!.Value.OrderID == Id)
                                         select new BO.OrderItem()
                                         {
                                             ID = orderItem.Value.ID,
                                             ProductID = orderItem.Value.ProductID,
                                             OrderID = orderItem.Value.OrderID,
                                             ProductName = Dal.Product.Get(x => x.Value.ID == orderItem.Value.ProductID)!.Value.Name,
                                             ProductPrice = Dal.Product.Get(x => x.Value.ID == orderItem.Value.ProductID)!.Value.Price,
                                             Amount = orderItem.Value.Amount,
                                             TotalPrice = orderItem.Value.Amount * Dal.Product.Get(x => x.Value.ID == orderItem.Value.ProductID)!.Value.Price
                                         }
                                     )
                                            )
                                 );


        boOrder.Details = boOrderDetailsTuple.FirstOrDefault().Item2.ToList();
        boOrder.TotalPrice = boOrderDetailsTuple.FirstOrDefault().TotalPrice;

        return boOrder;
    }

    /// return a list with all orders
    /// <returns> order list </returns>
    public IEnumerable<BO.OrderForList> GetList()
    {
        return from order in Dal!.Order.GetAll()
               select new BO.OrderForList()
               {
                   ID = order.Value.ID,
                   CustomerName = order.Value.CustomerName,
                   OrderStatus = CheckStatus(order),
                   Amount = GetPriceAndAmount(order.Value.ID).First().Item2,
                   TotalPrice = GetPriceAndAmount(order.Value.ID).First().Item3
               };
    }

    private IEnumerable<(DO.OrderItem?, int, double)> GetPriceAndAmount(int orderID)
    {
        return from orderItem in Dal!.OrderItem.GetAll(x => x.Value.OrderID == orderID)
               let a = orderItem.Value.Amount
               let b = (orderItem.Value.Amount * orderItem.Value.Price)
               select (orderItem, a, b);
    }

    ///search for a order with specific Id 
    /// <returns> IBoOrder item </returns>
    public BO.Order Get(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");
        try { return ConvertDoOrderToBoOrder(Id); }
        catch (IdException) { throw new BO.IdBOException("order with given Id didn't found"); }
    }

    ///search for a order that has not shipped yet with specific Id 
    ///update shipping date, and returns updated order
    public BO.Order UpdateShipping(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id! .(BO.Order.UpdateShipping)");
        foreach (DO.Order item in Dal!.Order.GetAll(x => x!.Value.ID == Id))
        {
            if (item.ShipDate < DateTime.Now) throw new BO.IdBOException("order has already shipped");
            else if (item.OrderDate > DateTime.Now) throw new BO.IdBOException("order has not ordered yet");
            else if (item.ShipDate > DateTime.Now && item.OrderDate < DateTime.Now)
            {
                DO.Order order = new()
                {
                    ID = item.ID,
                    CustomerName = item.CustomerName,
                    CustomerEmail = item.CustomerEmail,
                    CustomeAdress = item.CustomeAdress,
                    OrderDate = item.OrderDate ?? null,
                    ShipDate = DateTime.Now,
                    DeliveryDate = item.DeliveryDate ?? null
                };
                try
                {
                    Dal.Order.Update(order.ID, order);
                    return ConvertOrderToBoOrder(order);
                }
                catch (IdException) { throw new BO.IdBOException("Order exist. Impossible to update."); }
            }
        }
    throw new BO.IdBOException("order with given Id didn't found");
}

    ///search for a order that has shipped but has not provided yet with specific Id 
    ///update providing date, and returns updated order
    public BO.Order UpdateProviding(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");
        foreach (DO.Order? item in Dal!.Order.GetAll(x => x!.Value.ID == Id))
        {
            if (item?.DeliveryDate < DateTime.Now) throw new BO.IdBOException("order has already provided");
            else if (item?.ShipDate > DateTime.Now) throw new BO.IdBOException("order has not shipped yet");
            else if (item?.DeliveryDate > DateTime.Now && item?.ShipDate < DateTime.Now)
            {
                DO.Order order = new()
                {
                    ID = item?.ID ?? 0,
                    CustomerName = item?.CustomerName,
                    CustomerEmail = item?.CustomerEmail,
                    CustomeAdress = item?.CustomeAdress,
                    OrderDate = item?.OrderDate,
                    ShipDate = item?.ShipDate,
                    DeliveryDate = DateTime.Now

                };
                try
                {
                    Dal.Order.Update(order.ID, order);
                    return ConvertOrderToBoOrder(order);
                }
                catch (IdException) { throw new BO.IdBOException("Order  exist. Impossible to update."); }
            }
        }
        throw new BO.IdBOException("order with given Id didn't found");
    }

    ///search for a order with specific Id 
    ///returns OrderTracking of this order
    public BO.OrderTracking OrderTracking(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");

        DO.Order order = Dal?.Order.Get(x => x?.ID == Id) ?? throw new DO.IdException("ID Order not found");
        OrderTracking orderTraking = new();
        orderTraking.OrderID = order.ID;
        orderTraking.Status = CheckStatus(order);
        Tuple<DateTime?, String?>? t1 = new Tuple<DateTime?, String?>(order.OrderDate, "Order approved");
        orderTraking.TupleList?.Add(t1);
        if (CheckStatus(order) == BO.Enums.Status.shiped)
            orderTraking.TupleList?.Add(new Tuple<DateTime?, String?>(order.OrderDate, "Order shipped"));
        if (CheckStatus(order) == BO.Enums.Status.provided)
            orderTraking.TupleList?.Add(new Tuple<DateTime?, String?>(order.OrderDate, "Order provided"));

        return orderTraking;
    }
}
