using BlApi;
using DalApi;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        private static readonly IDal? Dal = DalApi.Factory.Get();

        /// <summary>
        /// This method converts an object of type "DO.OrderItem" to an object of type "BO.OrderItem". 
        /// The method takes in a single parameter "DOTemp" which is of type "DO.OrderItem". 
        /// It creates a new object of type "BO.OrderItem" called "BOtemp" and assigns values to its properties based on the values of the properties of "DOTemp".
        /// </summary>
        /// <param name="DOTemp"></param>
        /// <returns></returns>
        private static BO.OrderItem ConvertDo2BoOrderItem(DO.OrderItem DOTemp)
        {
            BO.OrderItem BOtemp = new()
            {
                ProductID = DOTemp.ProductID,
                OrderID = DOTemp.ID,
                ProductName = Dal!.Product.Get(x => x?.ID == DOTemp.ID)?.Name,
                ProductPrice = DOTemp.Price,
                Amount = DOTemp.Amount
            };
            BOtemp.TotalPrice = (BOtemp.Amount * BOtemp.ProductPrice);

            return BOtemp;
        }

        /// <summary>
        /// This method converts an object of type "BO.OrderItem" to an object of type "DO.OrderItem". 
        /// The method takes in two parameters, "BOTemp" which is of type "BO.OrderItem" and "id" which is an integer. 
        /// It creates a new object of type "DO.OrderItem" called "DOtemp" and assigns values to its properties based on 
        /// the values of the properties of "BOTemp". 
        /// </summary>
        /// <param name="BOTemp"></param>
        /// <param name="id"></param>
        /// <returns>"DOtemp" object is returned.</returns>
        private static DO.OrderItem ConvertBo2DoOrderItem(BO.OrderItem BOTemp, int id)
        {
            DO.OrderItem DOtemp = new()
            {
                ID = BOTemp.ID,
                ProductID = BOTemp.ProductID,
                OrderID = id,
                Price = BOTemp.ProductPrice,
                Amount = BOTemp.Amount
            };

            return DOtemp;
        }

        /// <summary>
        /// This function converts an object of type "DO.Product" to an object of type "BO.Product".
        /// It takes in a single parameter "DOTemp" of type "DO.Product".
        /// It creates a new object "BOtemp" of type "BO.Product" and assigns values of properties 
        /// from "DOTemp" properties to "BOtemp" properties . It returns the "BOtemp" object.
        /// </summary>
        /// <param name="DOTemp"></param>
        /// <returns></returns>
        private static  BO.Product ConvertDo2BoProduct(DO.Product DOTemp)
        {
            BO.Product BOtemp = new()
            {
                ID = DOTemp.ID,
                Name = DOTemp.Name,
                Price = DOTemp.Price,
                Category = (BO.Category)DOTemp.Category!,
                InStock = DOTemp.InStock
            };

            return BOtemp;
        }

        /// <summary>
        /// This code contains two methods: ConvertDo2BoOrder and GetOrderStatus. 
        /// The ConvertDo2BoOrder method is used to convert an object of type DO.Order to an object of type BO.Order.
        /// It takes in a single parameter DOTemp which is of type DO.Order.
        /// </summary>
        /// <param name="DOTemp"></param>
        /// <returns></returns>
        private static BO.Order ConvertDo2BoOrder(DO.Order DOTemp)
        {
            // Create a new BO.Order object and set its properties using values from the DOTemp object
            BO.Order BOtemp = new()
            {
                ID = DOTemp.ID,
                CustomerName = DOTemp.CustomerName,
                CustomerEmail = DOTemp.CustomerEmail,
                CustomerAdress = DOTemp.CustomeAdress,
                OrderDate = DOTemp.OrderDate,
                ShipDate = DOTemp.ShipDate,
                DeliveryDate = DOTemp.DeliveryDate
            };

            // Get the status of the order and set it in the BOtemp object
            if (DOTemp.ShipDate < DOTemp.DeliveryDate) BOtemp.OrderStatus = BO.Status.Shipped;
            else if (DOTemp.ShipDate < DateTime.Today) BOtemp.OrderStatus = BO.Status.Shipped;
            else BOtemp.OrderStatus = BO.Status.Approved;

            // Create an empty list for the order details
            BOtemp.Details = new();

            return BOtemp;
        }

        ///add product to Cart, returns updated cart
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Cart Add(BO.Cart boCart, int productId)
        {

            //create a copy of current cart to return it
            List<BO.Product> productList = new();
            foreach (DO.Product? item in Dal!.Product.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add") ) 
                productList.Add(ConvertDo2BoProduct(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add")));

            List<BO.OrderItem> orderItemList = new();
            foreach (DO.OrderItem? item in Dal!.OrderItem.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add")) 
                orderItemList.Add(ConvertDo2BoOrderItem(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add")));

            List<BO.Order> orderList = new();
            foreach (DO.Order? item in Dal!.Order.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add")) 
                orderList.Add(ConvertDo2BoOrder(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add")));

            DO.Product? product1 = Dal.Product.Get(x => x.Value.ID == productId);
            if (product1?.InStock <= 0) throw new BO.InStockException("product not in stock!");

            BO.Cart newBoCart = new()
            {
                CustomerName = boCart.CustomerName,
                CustomerEmail = boCart.CustomerEmail,
                CustomeAdress = boCart.CustomeAdress,
                TotalPrice = boCart.TotalPrice,
                Details = boCart.Details.ToList()
            };
            //end of copy cart - end stage 1

            // Check if the product with the specified productId exists in the cart
            if (newBoCart.Details.Any(x => x?.ProductID == productId))
            {
                // Create a new collection of BO.OrderItem objects using a LINQ query
                // The query filters the productList and boCart.Details collections to select only the matching product and order item with the specified productId
                IEnumerable<BO.OrderItem> updateDetails =  from product in productList
                                                           from orderItem in boCart.Details
                                                           where (product.ID == productId && product.InStock > 0 && orderItem.ProductID == productId)
                                                           select new BO.OrderItem()
                                                                   {
                                                                       ID = orderItem.ID,
                                                                       ProductID = orderItem.ProductID,
                                                                       OrderID = orderItem.OrderID,
                                                                       ProductName = orderItem.ProductName,
                                                                       ProductPrice = orderItem.ProductPrice,
                                                                       Amount = orderItem.Amount + 1,
                                                                       TotalPrice = orderItem.TotalPrice + orderItem.ProductPrice
                                                                   };

                // Remove all instances of the product from the cart's Details property
                newBoCart.Details.RemoveAll(x => x?.ProductID == productId);
                // Add the newly created collection of BO.OrderItem objects to the Details property of the cart
                newBoCart.Details.AddRange(updateDetails);

            }
            
            else //if product is not yet inside cart
            {
                // Create a new collection of BO.OrderItem objects using a LINQ query
                // The query filters the productList and boCart.Details collections to select only the matching product with the specified productId and InStock > 0
                IEnumerable<BO.OrderItem> updateDetails = from product in productList
                                                          where (product.ID == productId && product.InStock > 0)
                                                          select new BO.OrderItem()
                                                          {
                                                              ID = 887799,
                                                              ProductID = product.ID,
                                                              //OrderID,
                                                              ProductName = product.Name,
                                                              ProductPrice = product.Price,
                                                              Amount = 1,
                                                              TotalPrice = product.Price
                                                          };

                // Add the first item from the new collection of BO.OrderItem objects to the Details property of the cart
                newBoCart.Details.Add(updateDetails.First());
            }

            // Recalculate the TotalPrice of the cart
            newBoCart.TotalPrice = 0;
            foreach (var item in newBoCart.Details)
            {
                newBoCart.TotalPrice += item!.TotalPrice;
            }
            
            return newBoCart;
        }
        
        ///updated the amount in the cart
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Cart UpdateAmount(BO.Cart boCart, int Id, int NewAmount)
        {
            // Create a new BO.Cart object with the same properties as the original boCart object
            BO.Cart newBoCart = new()
            {
                CustomerName = boCart.CustomerName,
                CustomerEmail = boCart.CustomerEmail,
                CustomeAdress = boCart.CustomeAdress,
                Details = boCart.Details,
                TotalPrice = boCart.TotalPrice
            };

            // Iterate over each item in the boCart.Details collection
            foreach (BO.OrderItem? item in boCart.Details)
            {
                if (item?.ProductID == Id)
                {
                    // Check if the new amount is the same as the current amount
                    if (NewAmount == item.Amount) return newBoCart;
                    // Check if the new amount is 0
                    else if (NewAmount == 0)
                    {
                        // Subtract the total price of the item from the newBoCart.TotalPrice
                        newBoCart.TotalPrice -= item.ProductPrice * item.Amount;
                        // Remove the item from the newBoCart.Details collection
                        newBoCart.Details.Remove(item);
                        return newBoCart;
                    }
                    // Check if the new amount is greater than the current amount
                    else if (NewAmount > item.Amount)
                    {
                        BO.OrderItem newOrderItem = item;
                        newBoCart.TotalPrice += (NewAmount - newOrderItem.Amount) * newOrderItem.ProductPrice;
                        newOrderItem.Amount = NewAmount;
                        newBoCart.Details.Remove(item);
                        newBoCart.Details.Add(newOrderItem);
                        return newBoCart;
                    }
                    // Check if the new amount is less than the current amount
                    else if (NewAmount < item.Amount)
                    {
                        BO.OrderItem newOrderItem = item;
                        newBoCart.TotalPrice -= (newOrderItem.Amount - NewAmount) * newOrderItem.ProductPrice;
                        newOrderItem.Amount = NewAmount;
                        newBoCart.Details.Remove(item);
                        newBoCart.Details.Add(newOrderItem);
                        return newBoCart;
                    }
                }
            }
            // Throw an exception if the item with the specified Id was not found in the boCart.Details collection
            throw new BO.IdBOException("Item not found");
        }


        /// <summary>
        /// This code is a method named "ConfirmCart" that takes in a Cart object, a Name, an Email, and an Address as parameters.
        /// The method first checks if the cart is empty and throws an exception if it is. 
        /// It then iterates over each item in the cart's details collection, checking for various conditions such as if the product exists in the database, 
        /// If all conditions are met, the code creates a new Order object with the customer's information, adds it to the database, and updates the product's stock in the data source. 
        /// It also converts the BO OrderItem to a DO OrderItem and adds it to the data source.
        /// </summary>
        /// <param name="boCart"></param>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="Addres"></param>
        /// <exception cref="BO.IdBOException"></exception>
        /// <exception cref="BO.nullObjectBOException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ConfirmCart(BO.Cart boCart, string Name, string Email, string Addres)
        {
            // Check if the cart is empty
            if (boCart.Details.Count == 0) throw new BO.IdBOException("cart can not be empty!");

            // Iterate over each item in the boCart.Details collection
            foreach (BO.OrderItem? item in boCart.Details)
            {
                // Check if the product exists in the database
                if (Dal!.Product.GetAll(x => x?.ID == item?.ID).Count() < 0) throw new BO.IdBOException("not all the products in cart exist");
                // Check if the item's Amount is negative
                if (item?.Amount <= 0) throw new BO.IdBOException("negative Amount");
                // Check if the item's Amount is more than the stock
                if (item?.Amount > Dal?.Product?.Get(x => x?.ID == item!.ProductID)?.InStock) throw new BO.IdBOException("not enough in stock");
                // Check if the customer name is empty or null
                if (boCart.CustomerName == "" || boCart.CustomerName == null) throw new BO.IdBOException("Customer Name is not empty");
                // Check if the customer address is empty or null
                if (boCart.CustomeAdress == "" || boCart.CustomeAdress == null) throw new BO.IdBOException("Customer address is empty");
                // Check if the customer email is empty or null
                if (boCart.CustomerEmail == "" || boCart.CustomerEmail == null) throw new BO.IdBOException("Customer email is not valid");
            }

            // Create a new DO.Order object with the customer's information
            DO.Order newOrder = new()
            {
                CustomerName = Name,
                CustomeAdress = Addres,
                CustomerEmail = Email,
                OrderDate = DateTime.Now
            };
            // Add the newOrder to the database and set the newOrder's ID
            newOrder.ID = Dal!.Order.Add(newOrder);


            // Iterate through each item in the Cart's details
            foreach (BO.OrderItem? item in boCart.Details)
            {
                // Convert the BO OrderItem to a DO OrderItem
                DO.OrderItem? newOrderItem = ConvertBo2DoOrderItem(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add"), newOrder.ID);
                // Get the product from the source using the product ID
                int id = (int)newOrderItem?.ProductID!;
                var productFromSource = Dal.Product.Get(x => x?.ID == id);

                // Create a new DO Product with updated information
                DO.Product dp = new()
                {
                    ID = id,
                    Name = productFromSource?.Name,
                    Price = (double)productFromSource?.Price!,
                    Category = productFromSource?.Category,
                    InStock = (int)productFromSource?.InStock! - (int)newOrderItem?.Amount!,
                };
                // Update the product in the data source
                Dal.Product.Update(dp);
                // Add the new OrderItem to the data source
                Dal.OrderItem.Add(newOrderItem ?? throw new BO.nullObjectBOException("null"));
            }
        }
    }
}
/// interface of product items for manager and client
