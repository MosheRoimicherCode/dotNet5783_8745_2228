using DalApi;
using DO;
using System;
using static Dal.DataSource;
using System.Linq;

namespace Dal;




///A class for connect with Order struck
internal class DalOrder : IOrder
{

    ///----------------------------------------------------
    ///----------------- CRUD functions -------------------


    public int Add(Order order) => DataSource.AddOrder(order);/// Add order to Data Base

    public Order Get(int OrderID)
    {
        foreach (var order in from Order order in _orderList
                               where order.ID.Equals(OrderID)
                               select order)
        {
            return order;
        }
        Order order2 = new Order();
        order2.ID = -999;
        return order2;
    }///search for order by Id and return the specific order

    public void Delete(int OrderID)  

    {
        foreach(Order order in _orderList)
        {
            if (order.ID.Equals(OrderID))
            {
                _orderList.Remove(order);
            }
        }

        ///if not found return a message
        throw new IdException("Not found ID. (DalOrderExeptiot.Delete)"); ;
    }///delete order from data base by Id

    public void Update(int OrderID, Order newOrder)
    {
        for (int i = 0; i < _orderList.Count; i++)
        {
            var order = _orderList[i];
            if (order.ID.Equals(OrderID))
            {
                int index = _orderList.IndexOf(order);
                _orderList.RemoveAt(index);
                _orderList.Insert(index, newOrder);
            }
            return;
        }
        
        ///if not found return a message
        throw new IdException("Not found ID. (DalOrderExeption.Update)");
    }///replace order by another inside array


    ///----------------------------------------------------
    ///----------------------------------------------------



    ///----------------------------------------------------
    ///----------------- Methods --------------------------

    public List<Order> CopyList()
    {
        List<Order> orderlist = new List<Order>();
        orderlist = _orderList;
        return orderlist;
    }///return copy of order list

    public void GetAll()
    {
        foreach (Order order in _orderList)
        {
            if (order.ID != 0)
            {
                Console.WriteLine(order.ToString());
            }
        }
    }///ToString call for all list


    ///----------------------------------------------------


}