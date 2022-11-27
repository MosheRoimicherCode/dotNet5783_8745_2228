using BlApi;
using Dal;
using DalApi;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace BlImplementation
{
    internal class BoOrder : IBoOrder
    {
        IDal Dal = new DalList();

        ///checking the status of the order, returns Enum-status type
        public BO.Enums.Status CheckStatus(DO.Order o)
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

        ///Convert from Order To BoOrder
        public BO.BoOrder ConvertOrderToBoOrder(int Id)
        {
            DO.Order dalOrder = Dal.Order.Get(Id);
            BO.BoOrder boOrder = new BO.BoOrder();
            boOrder.ID = dalOrder.ID;
            boOrder.CustomerName = dalOrder.CustomerName;
            boOrder.CustomerEmail = dalOrder.CustomerEmail;
            boOrder.CustomeAdress = dalOrder.CustomeAdress;
            boOrder.OrderDate = dalOrder.OrderDate;
            boOrder.ShipDate = dalOrder.ShipDate;
            boOrder.DeliveryDate = dalOrder.DeliveryDate;
            List <DO.OrderItem> dalOlist = Dal.OrderItem.CopyList();
            List<DO.OrderItem> boOlist = new List<DO.OrderItem>();
            foreach (DO.OrderItem item in dalOlist)
            {
                if (item.OrderID == Id)
                {
                    boOlist.Add(item);
                }
            }
            boOrder.Details = boOlist;
            double price = 0;
            foreach (DO.OrderItem item in boOlist)
            {
                price += item.Price;
            }
            boOrder.TotalPrice = price;
            boOrder.OrderStatus = CheckStatus(dalOrder);

            return boOrder;
        }


        /// return a list with all orders
        /// <returns> order list </returns>
        public List<BO.BoOrderForList> GetList()
        {
            List<DO.Order> dalOlist = Dal.Order.CopyList();
            List<BO.BoOrderForList> boOlist = new List<BO.BoOrderForList>();
            foreach (DO.Order order in dalOlist)
            {
                BO.BoOrderForList boOrderForList = new BO.BoOrderForList();
                boOrderForList.ID = order.ID;
                boOrderForList.CustomerName = order.CustomerName;
                int count = 0;
                Double price = 0;
                List<DO.OrderItem> l = Dal.OrderItem.CopyList();
                foreach (DO.OrderItem item in l)
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

        ///search for a order with specific Id 
        /// <returns> IBoOrder item </returns>
        public BO.BoOrder Get(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id!");
            try
            {
                return ConvertOrderToBoOrder(Id);
            }
            catch (IdException) { throw new BO.IdBOException("order with given Id didn't found"); }
        }

        ///search for a order that has not shipped yet with specific Id 
        ///update shipping date, and returns updated order
        public BO.BoOrder UpdateShipping(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id!");

            List<DO.Order> dalOrder = Dal.Order.CopyList();
            foreach (DO.Order item in dalOrder)
            {
                if (item.ID == Id)
                {
                    if (item.ShipDate < DateTime.Now)
                    {
                        throw new BO.IdBOException("order has already shipped");
                    }
                    else if (item.ShipDate > DateTime.Now)
                    {
                        DO.Order o = new DO.Order(item.ID, item.CustomerName, item.CustomerEmail, item.CustomeAdress, item.OrderDate, DateTime.Now, item.DeliveryDate);
                        try
                        {
                            Dal.Order.Update(o.ID, o);
                            return ConvertOrderToBoOrder(Id);

                        }
                        catch (IdException) { throw new BO.IdBOException("Order exist. Impossible to update."); }
                    }
                }
            }
            throw new BO.IdBOException("order with given Id didn't found");
        }

        ///search for a order that has shipped but has not provided yet with specific Id 
        ///update providing date, and returns updated order
        public BO.BoOrder UpdateProviding(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id!");

            List<DO.Order> dalOrder = Dal.Order.CopyList();
            foreach (DO.Order item in dalOrder)
            {
                if (item.ID == Id)
                {
                    if (item.DeliveryDate < DateTime.Now)
                    {
                        throw new BO.IdBOException("order has already provided");
                    }
                    else if (item.ShipDate > DateTime.Now)
                    {
                        throw new BO.IdBOException("order has not shipped yet");
                    }
                    else if (item.DeliveryDate > DateTime.Now && item.ShipDate < DateTime.Now)
                    {
                        DO.Order o = new DO.Order(item.ID, item.CustomerName, item.CustomerEmail, item.CustomeAdress, item.OrderDate, item.ShipDate, DateTime.Now);
                        try
                        {
                            Dal.Order.Update(o.ID, o);
                            return ConvertOrderToBoOrder(Id);
                        }
                        catch (IdException) { throw new BO.IdBOException("Order exist. Impossible to update."); }
                    }
                }
            }
            throw new BO.IdBOException("order with given Id didn't found");
        }

        ///search for a order with specific Id 
        ///returns OrderTracking of this order
        public BO.BoOrderTracking OrderTracking(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id!");
            try
            {
                List<DO.Order> dalOrder = Dal.Order.CopyList();
                DO.Order dal = Dal.Order.Get(Id);
                BO.BoOrderTracking bo = new BO.BoOrderTracking();
                bo.OrderID = dal.ID;
                bo.Status = CheckStatus(dal);
                var t1 = new Tuple<DateTime, String>(dal.OrderDate,"Order approved");
                //Tuple<DateTime, String> t1 = new Tuple<DateTime, String>; //(dal.OrderDate, "Order approved");
                //(DateTime, String) t1 = (dal.OrderDate, "Order approved");
                bo.TupleList = t1;
                if (CheckStatus(dal) == BO.Enums.Status.shiped)
                {
                    Tuple<DateTime, String> t2 = new Tuple<DateTime, String>(dal.OrderDate, "Order shipped");
                    bo.TupleList = t2;
                }
                if (CheckStatus(dal) == BO.Enums.Status.provided)
                {
                    Tuple<DateTime, String> t3 = new Tuple<DateTime, String>(dal.OrderDate, "Order provided");
                    bo.TupleList = t3;
                }
                return bo;
            }
            catch (IdException) { throw new BO.IdBOException("Order exist. Impossible to update."); }
        }
    }
}
