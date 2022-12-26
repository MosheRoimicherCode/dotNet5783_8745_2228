using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation
{
    internal class Order : BlApi.IOrder
    {
        IDal? Dal = DalApi.Factory.Get();

        ///checking the status of the order, returns Enum-status type
        public BO.Enums.Status CheckStatus(DO.Order? o)
        {
            if (o?.ShipDate > DateTime.Now)          return BO.Enums.Status.approved;
            else if (o?.DeliveryDate > DateTime.Now) return BO.Enums.Status.shiped;
            else if (DateTime.Now > o?.DeliveryDate) return BO.Enums.Status.provided;
            else                                     return BO.Enums.Status.error;
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
        public BO.Order ConvertDoOrderToBoOrder(int Id)
        {
            DO.Order? dalOrder = Dal.Order.Get(x => x?.ID == Id)!;
            BO.Order boOrder = new BO.Order();
            
            boOrder.ID = dalOrder.Value.ID;
            boOrder.CustomerName = dalOrder?.CustomerName;
            boOrder.CustomerEmail = dalOrder?.CustomerEmail;
            boOrder.CustomeAdress = dalOrder?.CustomeAdress;
            boOrder.OrderStatus = CheckStatus(dalOrder);
            boOrder.OrderDate = dalOrder?.OrderDate;
            boOrder.ShipDate = dalOrder?.ShipDate;
            boOrder.DeliveryDate = dalOrder?.DeliveryDate;
            boOrder.Details = new();
            boOrder.TotalPrice = 0;

            foreach (var x in Dal.OrderItem.GetAll(x => x.Value.OrderID == Id))  //convert do order item do bo;
            {
                BO.OrderItem item = new();

                item.ID = x.Value.ID;
                item.ProductID= x.Value.ProductID;
                item.OrderID = x.Value.OrderID;
                int a = x.Value.ProductID;
                item.ProductName = Dal.Product.Get(y => y.Value.ID == a)!.Value.Name;
                item.ProductPrice = Dal.Product.Get(y => y.Value.ID == a)!.Value.Price;
                item.Amount = x.Value.Amount;
                item.TotalPrice = item.Amount * item.ProductPrice;

                boOrder.Details.Add(item);
                boOrder.TotalPrice += x.Value.Price;
            }

            return boOrder;
        }

        /// return a list with all orders
        /// <returns> order list </returns>
        public List<BO.OrderForList> GetList()
        {
            List<DO.Order?> dalOrderlist = Dal.Order.GetAll().ToList();
            List<BO.OrderForList?> boOlist = new();
            BO.OrderForList boOrderForList = new();
            foreach (DO.Order order in dalOrderlist)
            {
                boOrderForList.ID = order.ID;
                boOrderForList.CustomerName = order.CustomerName;
                boOrderForList.OrderStatus = CheckStatus(order);

                int count = 0;
                double price = 0;

                List<DO.OrderItem?> OrderItemList = Dal.OrderItem.GetAll().ToList();
          
                foreach (DO.OrderItem item in OrderItemList)
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
            return boOlist!;
        }

        ///search for a order with specific Id 
        /// <returns> IBoOrder item </returns>
        public BO.Order Get(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Negative Id!");
            try
            {
                return ConvertDoOrderToBoOrder(Id);
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

            foreach (DO.Order? item in OrderList)
            {
                if (item?.ID == Id)
                {
                    if (item?.DeliveryDate < DateTime.Now)
                    {
                        throw new BO.IdBOException("order has already provided");
                    }
                    else if (item?.ShipDate > DateTime.Now)
                    {
                        throw new BO.IdBOException("order has not shipped yet");
                    }
                    else if (item?.DeliveryDate > DateTime.Now && item?.ShipDate < DateTime.Now)
                    {
                        DO.Order order = new()
                        {
                            ID = item?.ID?? 0,
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
            catch (IdException) { };
        }
    
    }
}
