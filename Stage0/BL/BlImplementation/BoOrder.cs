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

        public List<BoOrderForList> GetLists()
        {
            List<Order> dalOlist = Dal.Order.CopyOrderList();
            List<BoOrderForList> boOlist = new List<BoOrderForList>();
            foreach (Order order in dalOlist)
            {
                BoOrderForList boOrderForList = new BoOrderForList();
                boOrderForList.ID = order.ID;
                boOrderForList.CustomerName = order.CustomerName;
                int count = 0;
                Double price = 0;
                List<OrderItem> l = Dal.OrderItem.CopyOrderItemArray();
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
                boOlist.Add(boOrderForList);
            }
            return boOlist;
        }

        public BO.BoOrder Get(int Id)
        {
            if (Id <= 0) throw new IdBOException("Negative Id!");
            try
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
                List<OrderItem> dalOlist = Dal.OrderItem.CopyOrderItemArray();
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
                return boOrder;
            }
            catch (IdException) { throw new IdBOException("order with given Id didn't found"); }
        }


        public BO.BoProductItem Get(int Id, BO.BoCart cart)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(BO.BoProduct item)
        {
            throw new NotImplementedException();
        }

        public BO.BoProductForList GetLists()
        {
            BO.BoProductForList product = new BO.BoProductForList();
            return product;
        }
    }
}
