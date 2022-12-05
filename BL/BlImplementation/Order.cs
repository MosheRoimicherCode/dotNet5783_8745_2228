using BlApi;
using Dal;
using DalApi;

namespace BlImplementation
{
    internal class Order : BlApi.IOrder
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
            else if (DateTime.Now > o.DeliveryDate)
            {
                return BO.Enums.Status.provided;
            }
            else
            {
                return BO.Enums.Status.error;
            }

        }

        ///Convert from Order To BoOrder
        public BO.Order ConvertOrderToBoOrder(DO.Order o)
        {
            BO.Order bo = new BO.Order();
            bo.ShipDate = o.ShipDate;
            bo.DeliveryDate = o.DeliveryDate;
            bo.OrderDate = o.OrderDate;
            bo.CustomerName = o.CustomerName;
            bo.CustomeAdress = o.CustomeAdress;
            bo.CustomerEmail = o.CustomerEmail;
            bo.OrderStatus = CheckStatus(o);
            return bo;
        }
        public BO.Order ConvertOrderToBoOrder(int Id)
        {
            DO.Order dalOrder = Dal.Order.Get(Id);
            BO.Order boOrder = new BO.Order();

            boOrder.ID = dalOrder.ID;
            boOrder.CustomerName = dalOrder.CustomerName;
            boOrder.CustomerEmail = dalOrder.CustomerEmail;
            boOrder.CustomeAdress = dalOrder.CustomeAdress;
            boOrder.OrderDate = dalOrder.OrderDate;
            boOrder.ShipDate = dalOrder.ShipDate;
            boOrder.DeliveryDate = dalOrder.DeliveryDate;
            List <DO.OrderItem> dalOlist = Dal.OrderItem.CopyList();
            List<DO.OrderItem?> boOlist = new List<DO.OrderItem?>();
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
        public List<BO.OrderForList> GetList()
        {
            List<DO.Order> dalOlist = Dal.Order.CopyList();
            List<BO.OrderForList> boOlist = new List<BO.OrderForList>();
            foreach (DO.Order order in dalOlist)
            {
                BO.OrderForList boOrderForList = new BO.OrderForList();
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
        public BO.Order Get(int Id)
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
        public BO.Order UpdateShipping(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id! .(BO.Order.UpdateShipping)");

            List<DO.Order?> OrderList = new();
            foreach (DO.Order? order in Dal.Order.GetAll())
                OrderList.Add(order ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));

            foreach (DO.Order item in OrderList)
            {

                if (item.ID == Id)
                    {
                    if (item.ShipDate < DateTime.Now)
                    {
                        throw new BO.IdBOException("order has already shipped");
                    }
                    else if (item.OrderDate > DateTime.Now)
                    {
                        throw new BO.IdBOException("order has not ordered yet");
                    }
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
        }
        throw new BO.IdBOException("order with given Id didn't found");
    }

        ///search for a order that has shipped but has not provided yet with specific Id 
        ///update providing date, and returns updated order
        public BO.Order UpdateProviding(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id!");

            List<DO.Order?> OrderList = new();
            foreach (DO.Order? order in Dal.Order.GetAll())
                OrderList.Add(order ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));

            foreach (DO.Order item in OrderList)
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
                        DO.Order order = new()
                        {
                            ID = item.ID,
                            CustomerName = item.CustomerName,
                            CustomerEmail = item.CustomerEmail,
                            CustomeAdress = item.CustomeAdress,
                            OrderDate = item.OrderDate ?? null,
                            ShipDate = item.ShipDate ?? null,
                            DeliveryDate = DateTime.Now

                        };

                        try
                        {
                            Dal.Order.Update(order.ID, order);
                            return ConvertOrderToBoOrder(order);
                        }
                        catch (IdException) { throw new BO.IdBOException("Order exist. Impossible to update."); }
                    }
                }
            }
            throw new BO.IdBOException("order with given Id didn't found");
        }

        ///search for a order with specific Id 
        ///returns OrderTracking of this order
        public BO.OrderTracking OrderTracking(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id!");
            try
            {

            List<DO.Order?> OrderList = new();
            foreach (DO.Order? dalOrder in Dal.Order.GetAll())
                OrderList.Add(dalOrder ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));

            DO.Order order = Dal.Order.Get(Id);
            BO.OrderTracking bo = new BO.OrderTracking();
            bo.OrderID = order.ID;
            bo.Status = CheckStatus(order);
            Tuple<DateTime?, String?>? t1 = new Tuple<DateTime?, String?>(order.OrderDate, "Order approved");
            //Tuple<DateTime, String> t1 = new Tuple<DateTime, String>; //(dal.OrderDate, "Order approved");
            //(DateTime, String) t1 = (dal.OrderDate, "Order approved");
            bo.TupleList = t1;
            if (CheckStatus(order) == BO.Enums.Status.shiped)
            {
                Tuple<DateTime?, String?>? t2 = new Tuple<DateTime?, String?>(order.OrderDate, "Order shipped");
                bo.TupleList = t2;
            }
            if (CheckStatus(order) == BO.Enums.Status.provided)
            {
                bo.TupleList = new Tuple<DateTime?, String?>(order.OrderDate, "Order provided");
            }
            return bo;
        }
        catch (IdException) { throw new BO.IdBOException("Order exist. Impossible to update."); }
        }
    
    }
}
