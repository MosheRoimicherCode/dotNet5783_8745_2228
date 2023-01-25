﻿namespace BlImplementation;

using BlApi;
using BO;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

internal class Order : BlApi.IOrder
{
    static readonly IDal dal = DalApi.Factory.Get()!;

    ///checking the status of the order, returns Enum-status type
    private static BO.Status CheckStatus(DO.Order? order)
    {
        if (order?.DeliveryDate != null) { return BO.Status.Provided; }
        else if (order?.ShipDate != null) { return BO.Status.Shipped; }
        else { return BO.Status.Approved; }
    }

    private static BO.Order ConvertOrderToBoOrder(DO.Order o)
    {
        return new BO.Order
        {
            ID = o.ID,
            CustomerName = o.CustomerName,
            CustomerEmail = o.CustomerEmail,
            CustomerAdress = o.CustomeAdress,
            OrderStatus = CheckStatus(o),
            OrderDate = o.OrderDate,
            ShipDate = o.ShipDate,
            DeliveryDate = o.DeliveryDate,
            TotalPrice = GetTotalPrice(o.ID),
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
        var a = boOrder.OrderStatus;
        var boOrderDetailsTuple = from orderItem in dal!.OrderItem.GetAll(x => x?.OrderID == Id)
                                   let TotalPrice = boOrder.TotalPrice + dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Price
                                   select (TotalPrice, new List<BO.OrderItem>
                                                (from orderItem in dal!.OrderItem.GetAll(x => x?.OrderID == Id)
                                                 select new BO.OrderItem()
                                                 {
                                                     ID = (int)orderItem?.ID!,
                                                     ProductID = (int)orderItem?.ProductID!,
                                                     OrderID = (int)orderItem?.OrderID!,
                                                     ProductName = dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Name,
                                                     ProductPrice = dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Price ?? 0,
                                                     Amount = (int)orderItem?.Amount!,
                                                     TotalPrice = (int)orderItem?.Amount! * (double)dal.Product.Get(x => x?.ID == orderItem?.ProductID)?.Price!,
                                                 }));

        try {  boOrder.Details = boOrderDetailsTuple.FirstOrDefault().Item2.ToList(); } catch { }
        IEnumerable<DO.OrderItem?> orderItem1 = dal!.OrderItem.GetAll();
       
        boOrder.TotalPrice = boOrderDetailsTuple.FirstOrDefault().TotalPrice ?? 0;

        return boOrder;
    }
    private static IEnumerable<(DO.OrderItem?, int, double)> GetPriceAndAmount(int orderID) =>
        from orderItem in dal.OrderItem.GetAll(x => x?.OrderID == orderID)
        let a = (int)orderItem?.Amount!
        let b = ((int)orderItem?.Amount! * (double)orderItem?.Price!)
        select (orderItem, a, b);
    private static double GetTotalPrice(int orderID)
    {
        double price = 0;
        foreach (var item in dal.OrderItem.GetAll(x => x?.OrderID == orderID))
        {
            price += item.Value.Price;
        }
        return price;
    }
    private static int GetAmount(int orderID)
    {
        int amount = 0;
        foreach (var item in dal.OrderItem.GetAll(x => x?.OrderID == orderID))
        {
            amount += item.Value.Amount;
        }
        return amount;
    }

    /// return a list with all orders
    /// <returns> order list </returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderForList> GetList()
    {
        IEnumerable<DO.OrderItem?> all = dal.OrderItem.GetAll();
        try
        {
            return from order in dal.Order.GetAll()
                   let Id = order?.ID ?? 0
                   select new BO.OrderForList()
                   {
                       ID = Id,
                       CustomerName = order?.CustomerName,
                       OrderStatus = CheckStatus(order),
                       Amount = GetAmount(Id),
                       TotalPrice = GetTotalPrice(Id)
                   };
        }
        catch { return null!; }
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
            if (item?.ShipDate != null) throw new BO.IdBOException("order has already shipped");
            else if (item?.OrderDate == null) throw new BO.IdBOException("order has not ordered yet");
            else if (item?.ShipDate == null && item?.OrderDate != null)
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
                    IEnumerable<DO.OrderItem?> u = dal.OrderItem.GetAll();
                    return ConvertOrderToBoOrder(order);
                }
                catch (IdException) { throw new BO.IdBOException("Order exist. Impossible to update."); }
            }
        }
        throw new BO.IdBOException("order with given Id didn't found");
    }

    ///search for a order that has shipped but has not provided yet with specific Id 
    ///update providing date, and returns updated order
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateProviding(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");
        else
        {
            foreach (DO.Order? item in dal!.Order.GetAll(x => x?.ID == Id))
            {
                if (item?.DeliveryDate != null) throw new BO.IdBOException("order has already provided");
                else if (item?.ShipDate == null) throw new BO.IdBOException("order has not shiped yet");
                else if (item?.DeliveryDate == null && item?.ShipDate != null)
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
                    dal.Order.Update(order);
                    return ConvertOrderToBoOrder(order);
                }
            }
        }
        throw new BO.IdBOException("order with given Id didn't found");
    }

    ///search for a order with specific Id 
    ///returns OrderTracking of this order
    [MethodImpl(MethodImplOptions.Synchronized)]
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

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderTracking> GetListOfTruckings() => from item in dal?.Order.GetAll()
                                                                 select OrderTracking((int)item?.ID!);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? ReturnOrderForManage()
    {

        var orderData = (from order in dal.Order.GetAll()
                         where (order?.DeliveryDate == null && order?.ShipDate == null)
                         orderby (order?.OrderDate)
                         select order).FirstOrDefault(); //oldest order without managin

        var ShipData = (from order in dal.Order.GetAll()
                        where (order?.DeliveryDate == null && order?.ShipDate != null)
                        orderby (order?.ShipDate)
                        select order).FirstOrDefault(); //oldest order without managin

        if (orderData != null && ShipData != null)
        {
            switch (orderData?.OrderDate < ShipData?.ShipDate)
            {
                case true:
                    return orderData?.ID;
                case false:
                    return ShipData?.ID;
            }
        }
        else if (orderData != null && ShipData == null) return orderData?.ID;
        else if (orderData == null && ShipData != null) return ShipData?.ID;
        else return null;


    } //return last updated order status

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateStatus(int id)
    {
        string? status = Get(id).OrderStatus.ToString();
        if (status == "Approved")
            return UpdateShipping(id);
        else { return UpdateProviding(id); }
    }

    public void Delete(int id)
    {
        dal.Order.Delete(id);
    }
}
