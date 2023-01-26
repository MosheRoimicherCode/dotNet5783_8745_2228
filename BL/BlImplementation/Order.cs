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

    /// <summary>
    /// checking the status of the order, and returns Status type
    /// </summary>
    /// <param name="order"></param>
    /// <returns> BO.Status </returns>
    private static BO.Status CheckStatus(DO.Order? order)
    {
        if (order?.DeliveryDate != null) { return BO.Status.Provided; }
        else if (order?.ShipDate != null) { return BO.Status.Shipped; }
        else { return BO.Status.Approved; }
    }

    /// <summary>
    /// This function converts a DO.Order object to a BO.Order object. 
    /// It assigns the values of the properties of the input object to the corresponding properties of the new object.
    /// This function converts a DO.Order object to a BO.Order object. 
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>s to set the OrderStatus and TotalPrice properties respectively.
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

    /// <summary>
    /// This code is a C# function that converts an order from the data access layer (DAL) to a business object (BO) representation.
    /// The function starts by retrieving an order from the DAL by calling the "Get" method with a lambda expression to filter the order by its ID.
    ///Then it creates a new instance of the BO.Order object, and assigns values to its properties. 
    ///Finally, it returns the BO.Order object
    /// </summary>
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

    /// <summary>
    /// This function is used to retrieve all the order items of a specific order by the order ID, and then it calculates the amount 
    /// and total price of each item using the let clause. Finally, it returns a collection of tuples that contain the order item, amount, and total price.
    /// this function it useful for further processing or displaying on the UI.
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns> IEnumerable<(DO.OrderItem?, int, double) </returns>
    private static IEnumerable<(DO.OrderItem?, int, double)> GetPriceAndAmount(int orderID) =>
        from orderItem in dal.OrderItem.GetAll(x => x?.OrderID == orderID)
        let a = (int)orderItem?.Amount!
        let b = ((int)orderItem?.Amount! * (double)orderItem?.Price!)
        select (orderItem, a, b);

    ///This function takes in an order ID as a parameter and calculates the total price of the order
    ///by iterating through all items in the order and adding their prices together.
    ///It then returns the total price as a double.
    private static double GetTotalPrice(int orderID)
    {
        double price = 0;
        foreach (var item in dal.OrderItem.GetAll(x => x?.OrderID == orderID))
        {
            price += item.Value.Price;
        }
        return price;
    }

    /// <summary>
    /// This function is used to retrieve the amount of items in an order
    /// </summary>
    /// <param name="orderID"></param>
    /// <returns></returns>kes in an orderID as a parameter, and returns the total amount of items in the order.
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
        // Get all order items from data access layer
        IEnumerable<DO.OrderItem?> all = dal.OrderItem.GetAll();
        try
        {
            // Select all orders, get the ID, and return a new BO.OrderForList object
            return from order in dal.Order.GetAll()
                   let Id = order?.ID ?? 0
                   select new BO.OrderForList()
                   {
                       ID = Id,
                       CustomerName = order?.CustomerName,
                       // Check the status of the order
                       OrderStatus = CheckStatus(order),
                       // Get the amount of items in the order
                       Amount = GetAmount(Id),
                       // Get the total price of the order
                       TotalPrice = GetTotalPrice(Id)
                   };
        }
        catch { return null!; }
    }

    ///search for a order with specific Id 
    /// This method retrieves an order from the database using its Id.
    /// If the Id is negative, an exception is thrown.
    /// If the order is not found in the database, an exception is thrown.
    /// The method also converts the retrieved order from its data object representation to its business object representation
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order Get(int Id)
    {
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");
        try { return ConvertDoOrderToBoOrder(Id); }
        catch (IdException) { throw new BO.IdBOException("order with given Id didn't found"); }
    }

    /// <summary>
    /// This method updates the shipping date of an order with the given ID. 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    /// <exception cref="BO.IdBOException"></exception>
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

    /// <summary>
    /// This code updates the delivery date of an order with the given ID.
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    /// <exception cref="BO.IdBOException"></exception>   [MethodImpl(MethodImplOptions.Synchronized)]
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateProviding(int Id)
    {
        // check if the Id is valid (greater than 0)
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");
        else
        {
            // iterate over all the orders in the DAL layer
            foreach (DO.Order? item in dal!.Order.GetAll(x => x?.ID == Id))
            {
                // check if the order has already been provided
                if (item?.DeliveryDate != null) throw new BO.IdBOException("order has already provided");
                // check if the order has not been shipped yet
                else if (item?.ShipDate == null) throw new BO.IdBOException("order has not shiped yet");
                // check if the delivery date is null and the ship date is not null
                else if (item?.DeliveryDate == null && item?.ShipDate != null)
                {
                    // create a new Order object with the updated delivery date
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
                    // update the order in the DAL layer
                    dal.Order.Update(order);
                    // convert the order to a BO.Order object and return it
                    return ConvertOrderToBoOrder(order);
                }
            }
        }
        // if the order with the given Id is not found, throw an exception
        throw new BO.IdBOException("order with given Id didn't found");
    }

    /// <summary>
    /// This function is used to track the status of an order given its ID.
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>it assigns the list of Tuples to the OrderTracking object and returns it.</returns>
    /// <exception cref="BO.IdBOException"></exception>
    /// <exception cref="DO.IdException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderTracking OrderTracking(int Id)
    {
        // check if the Id is valid (greater than 0)
        if (Id <= 0) throw new BO.IdBOException("Negative Id!");

        //copy the order with specific id, if not recognized id, up a throw message
        DO.Order order = dal?.Order.Get(x => x?.ID == Id) ?? throw new DO.IdException("ID Order not found");

        //create a orderTrakiv object
        OrderTracking orderTraking = new()
        {
            OrderID = order.ID,
            Status = CheckStatus(order)
        };

        //create tuple with orderDate and with string status "Order Approved"
        Tuple<DateTime?, string?>? t1 = new(order.OrderDate, "Order approved");

        //create now a list of DateTiem and String
        List<Tuple<DateTime?, string?>?>? tupleList1 = new() { t1 };

        //and add to it other objcts if thei corrspond the requiriment
        //if shipped
        if (CheckStatus(order) == BO.Status.Shipped)
            tupleList1.Add(new Tuple<DateTime?, String?>(order.OrderDate, "Order shipped"));

        //if provided
        if (CheckStatus(order) == BO.Status.Provided)
        {
            tupleList1.Add(new Tuple<DateTime?, String?>(order.ShipDate, "Order shipped"));
            tupleList1.Add(new Tuple<DateTime?, String?>(order.DeliveryDate, "Order provided"));
        }

        //assign the new created tupleList to main object.TupleList
        orderTraking.TupleList = tupleList1;

        //return main object
        return orderTraking;
    }

    /// <summary>
    /// create order object from data base and return it like orderTraking object
    /// work with link method
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderTracking> GetListOfTruckings() => from item in dal?.Order.GetAll()
                                                                 select OrderTracking((int)item?.ID!);
    /// <summary>
    /// return the last managed order.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? ReturnOrderForManage()
    {

        //serach all order that are just aproved from managr but not have n=beed shipped. then order them by date
        var orderData = (from order in dal.Order.GetAll()
                         where (order?.DeliveryDate == null && order?.ShipDate == null)
                         orderby (order?.OrderDate)
                         select order).FirstOrDefault(); 

        //serach all order that are just shiped but not delivered yet from managr. then order them by date
        var ShipData = (from order in dal.Order.GetAll()
                        where (order?.DeliveryDate == null && order?.ShipDate != null)
                        orderby (order?.ShipDate)
                        select order).FirstOrDefault(); 

        //check if we have both type of order to manage
        //is yex, check the earlier one and manage
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

        //in the oposite case, just manage the older one.
        else if (orderData != null && ShipData == null) return orderData?.ID;
        else if (orderData == null && ShipData != null) return ShipData?.ID;

        //if all order have been managed and delivered ti client, return null
        else return null;

    }

    /// <summary>
    /// check what next step of order status and update to it.
    /// return the if of order updated
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateStatus(int id)
    {
        //get the status of order by specifically id
        string? status = Get(id).OrderStatus.ToString();

        //if aproved, send to UpdateShipping function
        if (status == "Approved") return UpdateShipping(id);

        //else, send to UpdateProviding function
        else { return UpdateProviding(id); }
    }

    /// <summary>
    /// delete function
    /// /// </summary>
    /// <param name="id"></param>
    public void Delete(int id) => dal.Order.Delete(id);
}
