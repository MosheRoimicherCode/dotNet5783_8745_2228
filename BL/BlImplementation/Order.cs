namespace BlImplementation;
using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

internal class Order : BlApi.IOrder
{
    static readonly IDal dal = DalApi.Factory.Get()!;

    ///checking the status of the order, returns Enum-status type
    private static BO.Status CheckStatus(DO.Order? order) => order! switch
    {
        { DeliveryDate: not null } => BO.Status.Provided,
        { ShipDate: not null } => BO.Status.Shipped,
        _ => BO.Status.Approved
    };
    private static BO.Order ConvertOrderToBoOrder(DO.Order o)
    {
        return new BO.Order
        {
            ShipDate = o.ShipDate,
            DeliveryDate = o.DeliveryDate,
            OrderDate = o.OrderDate,
            CustomerName = o.CustomerName,
            CustomerAdress = o.CustomeAdress,
            CustomerEmail = o.CustomerEmail,
            OrderStatus = CheckStatus(o)
        };

    }
    private static BO.Order ConvertDoOrderToBoOrder(int Id)
    {
        DO.Order? dalOrder = dal?.Order.Get(x => x?.ID == Id)!;
        BO.Order boOrder = new()
        {
            ID = (int)dalOrder?.ID!,
            TotalPrice = 0,
            CustomerName = dalOrder?.CustomerName,
            CustomerEmail = dalOrder?.CustomerEmail,
            CustomerAdress = dalOrder?.CustomeAdress,
            OrderStatus = CheckStatus(dalOrder),
            OrderDate = dalOrder?.OrderDate,
            ShipDate = dalOrder?.ShipDate,
            DeliveryDate = dalOrder?.DeliveryDate,
            Details = new(),
        };
        var boOrderDetailsTuple = (from orderItem in dal!.OrderItem.GetAll(x => x?.OrderID == Id)
                                   let TotalPrice = boOrder.TotalPrice + dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Price
                                   select (TotalPrice, new List<BO.OrderItem>
                                     (
                                         from orderItem in dal!.OrderItem.GetAll(x => x?.OrderID == Id)
                                         select new BO.OrderItem()
                                         {
                                             ID = (int)orderItem?.ID!,
                                             ProductID = (int)orderItem?.ProductID!,
                                             OrderID = (int)orderItem?.OrderID!,
                                             ProductName = dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Name,
                                             ProductPrice = dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Price ?? 0,
                                             Amount = (int)orderItem?.Amount!,
                                             TotalPrice = (int)orderItem?.Amount! * (double)dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Price!,
                                         }
                                     )
                                            )
                                 );


        boOrder.Details = boOrderDetailsTuple.FirstOrDefault().Item2.ToList();
        boOrder.TotalPrice = boOrderDetailsTuple.FirstOrDefault().TotalPrice ?? 0;

        return boOrder;
    }
    private static IEnumerable<(DO.OrderItem?, int, double)> GetPriceAndAmount(int orderID) =>
        from orderItem in dal.OrderItem.GetAll(x => x?.OrderID == orderID)
        let a = (int)orderItem?.Amount!
        let b = ((int)orderItem?.Amount! * (double)orderItem?.Price!)
        select (orderItem, a, b);

    /// return a list with all orders
    /// <returns> order list </returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderForList> GetList()
    {
        List<BO.OrderForList> ordersForList = new();
        foreach (var item in dal.Order.GetAll())
        {
            int id = (int)item?.ID!;
            BO.OrderForList orderForList = new()
            {
                ID = id,
                CustomerName = item?.CustomerName,
                OrderStatus = CheckStatus(item)
            };
            try
            {
                orderForList.Amount = GetPriceAndAmount(id).First().Item2;
                orderForList.TotalPrice = GetPriceAndAmount(id).First().Item3;
            }
            catch (Exception)
            {

            }
            ordersForList.Add(orderForList);
        }
        return ordersForList;
        //return from order in Dal!.Order.GetAll()
        //       select new BO.OrderForList()
        //       {
        //           ID = order?.ID,
        //           CustomerName = order?.CustomerName ?? null,
        //           OrderStatus = CheckStatus(order),
        //           Amount = GetPriceAndAmount(order?.ID).First().Item2,
        //           TotalPrice = GetPriceAndAmount(order?.ID).First().Item3
        //       };
    }

    ///search for a order with specific Id 
    /// <returns> IBoOrder item </returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order Get(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");
        try { return ConvertDoOrderToBoOrder(Id); }
        catch (IdException) { throw new BO.IdBOException("order with given Id didn't found"); }
    }

    ///search for a order that has not shipped yet with specific Id 
    ///update shipping date, and returns updated order
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateShipping(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id! .(BO.Order.UpdateShipping)");
        foreach (var item in dal!.Order.GetAll(x => x?.ID == Id))
        {
            if (item?.ShipDate < DateTime.Now) throw new BO.IdBOException("order has already shipped");
            else if (item?.OrderDate > DateTime.Now) throw new BO.IdBOException("order has not ordered yet");
            else if (item?.ShipDate > DateTime.Now && item?.OrderDate < DateTime.Now)
            {
                DO.Order order = new()
                {
                    ID = (int)item?.ID!,
                    CustomerName = item?.CustomerName,
                    CustomerEmail = item?.CustomerEmail,
                    CustomeAdress = item?.CustomeAdress,
                    OrderDate = item?.OrderDate,
                    ShipDate = DateTime.Now,
                    DeliveryDate = item?.DeliveryDate
                };
                try
                {
                    dal.Order.Update(order);
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
        foreach (DO.Order? item in dal!.Order.GetAll(x => x?.ID == Id))
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
                    dal.Order.Update(order);
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

        DO.Order order = dal?.Order.Get(x => x?.ID == Id) ?? throw new DO.IdException("ID Order not found");
        OrderTracking orderTraking = new()
        {
            OrderID = order.ID,
            Status = CheckStatus(order)
        };
        Tuple<DateTime?, String?>? t1 = new(order.OrderDate, "Order approved");
        List<Tuple<DateTime?, string?>?>? tupleList1 = new() { t1 };
        if (CheckStatus(order) == BO.Status.Shipped)
            tupleList1.Add(new Tuple<DateTime?, String?>(order.OrderDate, "Order shipped"));
        if (CheckStatus(order) == BO.Status.Provided)
        {
            tupleList1.Add(new Tuple<DateTime?, String?>(order.ShipDate, "Order shipped"));
            tupleList1.Add(new Tuple<DateTime?, String?>(order.DeliveryDate, "Order provided"));
        }
        orderTraking.TupleList = tupleList1;
        return orderTraking;
    }

    public IEnumerable<BO.OrderTracking> GetListOfTruckings()
    {
        return from item in dal?.Order.GetAll()
               select OrderTracking((int)item?.ID!);
    }
}
