﻿using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;

///A class for connect with ORderItem struck
public class DalOrderItem
{

    ///----------------- Constructors ------------------- 
    public DalOrderItem(OrderItem o) => DataSource.AddOrderItem(o);


    ///----------------------------------------------------
    ///----------------- CRUD functions -------------------

    public void Add(OrderItem orderItem)
    {
        DataSource.AddOrderItem(orderItem);
    }/// Add order item item to Data Base

    public OrderItem Get(int ProductID, int OrderID)
    {
        foreach (OrderItem orderItem in _orderItemList)
        {

            if (orderItem.ProductID.Equals(ProductID) && orderItem.OrderID.Equals(OrderID))
            {
                return orderItem;
            }
        }
        ///in case of Id not found, throw exception
        throw new IdException();
    }///search for order item by Id's and return the specific order item

    public void Delete(int ProductID, int OrderID)
    {
        foreach (OrderItem orderItem in _orderItemList)
        {
            if (orderItem.ProductID.Equals(ProductID) && orderItem.OrderID.Equals(OrderID))
            {
                _orderItemList.Remove(orderItem);
            }
        }

        ///if not found return a message
        throw new IdException();
    }///delete order item from data base by Id's

    public void Update(int ProductID, int OrderID, OrderItem newOrderItem)
    {

        for (int i = 0; i < _orderItemList.Count; i++)
        {
            var orderItem = _orderItemList[i];
            if (orderItem.ProductID.Equals(ProductID) && orderItem.OrderID.Equals(OrderID))
            {
                int index = _orderItemList.IndexOf(orderItem);
                _orderItemList.RemoveAt(index);
                _orderItemList.Insert(index, newOrderItem);
            }
            return;
        }
       
        ///if not found return a message
        throw new IdException();
    }///replace order item by another inside array


    ///----------------------------------------------------
    ///----------------------------------------------------



    ///----------------------------------------------------
    ///----------------- Methods --------------------------


    public List<OrderItem> CopyOrderItemArray()
    {
        List<OrderItem> orderItemlist = new List<OrderItem>();
        orderItemlist = _orderItemList;
        return orderItemlist;
    }///return copy of orderItem list
    public void GetAll()
    {
        foreach (OrderItem orderItem in _orderItemList)
        {
            if (orderItem.OrderID != 0)
            {
                Console.WriteLine(orderItem.ToString());
            }
        }
    }///ToString call for all list

    ///----------------------------------------------------
}
