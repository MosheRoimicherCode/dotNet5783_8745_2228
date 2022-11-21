using BlApi;
using BO;
using Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace BlImplementation
{
    internal class BoOrder : IBoOrder
    {
        IDal Dal = new DalList();

        public BO.Enums.Status CheckStatus(Order o)
        {
            if (o.ShipDate > DateTime.Now)
            {
                return BO.Enums.Status.approved;
            }
            else if (o.DeliveryDate > DateTime.Now)
            {
                return BO.Enums.Status.shiped;
            }
            else
            {
                return BO.Enums.Status.provided;
            }

        }
        public BO.BoOrder ConvertOrderToBoOrder(int Id)
        {
            Order dalOrder = Dal.Order.Get(Id);
            BO.BoOrder boOrder = new BO.BoOrder();
            boOrder.ID = dalOrder.ID;
            boOrder.CustomerName = dalOrder.CustomerName;
            boOrder.CustomerEmail = dalOrder.CustomerEmail;
            boOrder.CustomeAdress = dalOrder.CustomeAdress;
            boOrder.OrderDate = dalOrder.OrderDate;
            boOrder.ShipDate = dalOrder.ShipDate;
            boOrder.DeliveryDate = dalOrder.DeliveryDate;
            List<OrderItem> dalOlist = Dal.OrderItem.CopyList();
            List<OrderItem> boOlist = new List<OrderItem>();
            foreach (OrderItem item in dalOlist)
            {
                if (item.OrderID == Id)
                {
                    boOlist.Add(item);
                }
            }
            boOrder.Details = boOlist;
            double price = 0;
            foreach (OrderItem item in boOlist)
            {
                price += item.Price;
            }
            boOrder.TotalPrice = price;
            boOrder.OrderStatus = CheckStatus(dalOrder);

            return boOrder;
        }

        public List<BoOrderForList> GetLists()
        {
            List<Order> dalOlist = Dal.Order.CopyList();
            List<BoOrderForList> boOlist = new List<BoOrderForList>();
            foreach (Order order in dalOlist)
            {
                BoOrderForList boOrderForList = new BoOrderForList();
                boOrderForList.ID = order.ID;
                boOrderForList.CustomerName = order.CustomerName;
                int count = 0;
                Double price = 0;
                List<OrderItem> l = Dal.OrderItem.CopyList();
                foreach (OrderItem item in l)
                {
                    if (item.OrderID == boOrderForList.ID)
                    {
                        count++;
                        price += item.Price;
                    }
                }
                boOrderForList.Amount = count;
                boOrderForList.TotalPrice = price;
                boOrderForList.OrderStatus = CheckStatus(order);

                boOlist.Add(boOrderForList);
            }
            return boOlist;
        }

        public BO.BoOrder Get(int Id)
        {
            if (Id <= 0) throw new IdBOException("Negative Id!");
            try
            {
                return ConvertOrderToBoOrder(Id);
            }
            catch (IdException) { throw new IdBOException("order with given Id didn't found"); }
        }

        public BO.BoOrder UpdateShipping(int Id)
        {
            if (Id <= 0) throw new IdBOException("Negative Id!");

            List<Order> dalOrder = Dal.Order.CopyList();
            foreach (Order item in dalOrder)
            {
                if (item.ID == Id)
                {
                    if (item.ShipDate < DateTime.Now)
                    {
                        throw new IdBOException("order has already shipped");
                    }
                    else if (item.ShipDate > DateTime.Now)
                    {
                        Order o = new Order(item.ID, item.CustomerName, item.CustomerEmail, item.CustomeAdress, item.OrderDate, DateTime.Now, item.DeliveryDate);
                        try
                        {
                            Dal.Order.Update(o.ID, o);
                            return ConvertOrderToBoOrder(Id);

                        }
                        catch (IdException) { throw new IdBOException("Order exist. Impossible to update."); }
                    }
                }
            }
            throw new IdBOException("order with given Id didn't found");
        }

        public BO.BoOrder UpdateProviding(int Id)
        {
            if (Id <= 0) throw new IdBOException("Negative Id!");

            List<Order> dalOrder = Dal.Order.CopyList();
            foreach (Order item in dalOrder)
            {
                if (item.ID == Id)
                {
                    if (item.DeliveryDate < DateTime.Now)
                    {
                        throw new IdBOException("order has already provided");
                    }
                    else if (item.ShipDate > DateTime.Now)
                    {
                        throw new IdBOException("order has not shipped yet");
                    }
                    else if (item.DeliveryDate > DateTime.Now && item.ShipDate < DateTime.Now)
                    {
                        Order o = new Order(item.ID, item.CustomerName, item.CustomerEmail, item.CustomeAdress, item.OrderDate, item.ShipDate, DateTime.Now);
                        try
                        {
                            Dal.Order.Update(o.ID, o);
                            return ConvertOrderToBoOrder(Id);
                        }
                        catch (IdException) { throw new IdBOException("Order exist. Impossible to update."); }
                    }
                }
            }
            throw new IdBOException("order with given Id didn't found");
        }

        public BO.BoOrderTracking OrderTracking(int Id)
        {
            if (Id <= 0) throw new IdBOException("Negative Id!");
            try
            {
                List<Order> dalOrder = Dal.Order.CopyList();
                Order dal = Dal.Order.Get(Id);
                BoOrderTracking bo = new BoOrderTracking();
                bo.OrderID = dal.ID;
                bo.Status = CheckStatus(dal);
                Tuple<DateTime, String> t1 = new Tuple<DateTime, String>(dal.OrderDate, "Order approved");
                bo.TupleList.Add(t1);
                if (CheckStatus(dal) == BO.Enums.Status.shiped)
                {
                    Tuple<DateTime, String> t2 = new Tuple<DateTime, String>(dal.OrderDate, "Order shipped");
                    bo.TupleList.Add(t2);
                }
                if (CheckStatus(dal) == BO.Enums.Status.provided)
                {
                    Tuple<DateTime, String> t3 = new Tuple<DateTime, String>(dal.OrderDate, "Order provided");
                    bo.TupleList.Add(t3);
                }
                return bo;
            }
            catch (IdException) { throw new IdBOException("Order exist. Impossible to update."); }
        }
    }
}
