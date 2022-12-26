using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        IDal? Dal = DalApi.Factory.Get();

        private BO.OrderItem ConvertDo2BoOrderItem(DO.OrderItem DOTemp)
        {
            BO.OrderItem BOtemp = new();

            BOtemp.ProductID = DOTemp.ProductID;
            BOtemp.OrderID = DOTemp.ID;
            BOtemp.ProductName = Dal.Product.Get(x => x?.ID == DOTemp.ID)?.Name;
            BOtemp.ProductPrice = DOTemp.Price;
            BOtemp.Amount = DOTemp.Amount;
            BOtemp.TotalPrice = (BOtemp.Amount * BOtemp.ProductPrice);

            return BOtemp;
        }
        private DO.OrderItem ConvertBo2DoOrderItem(BO.OrderItem BOTemp)
        {
            DO.OrderItem DOtemp = new();
            DOtemp.ID = BOTemp.ID;
            DOtemp.ProductID = BOTemp.ProductID;
            DOtemp.OrderID = BOTemp.ID;
            DOtemp.Price = BOTemp.ProductPrice;
            DOtemp.Amount = BOTemp.Amount;

            return DOtemp;
        }
        private BO.Product ConvertDo2BoProduct(DO.Product DOTemp)
        {
            BO.Product BOtemp = new();

            BOtemp.ID = DOTemp.ID;
            BOtemp.Name = DOTemp.Name;
            BOtemp.Price = DOTemp.Price;
            BOtemp.Category = (BO.Enums.Category)DOTemp.Category!;
            BOtemp.InStock = DOTemp.InStock;

            return BOtemp;
        }
        private BO.Order ConvertDo2BoOrder(DO.Order DOTemp)
        {
            BO.Order BOtemp = new();
            BOtemp.ID = DOTemp.ID;
            BOtemp.CustomerName = DOTemp.CustomerName;
            BOtemp.CustomerEmail = DOTemp.CustomerEmail;
            BOtemp.CustomeAdress = DOTemp.CustomeAdress;
            BOtemp.OrderDate = DOTemp.OrderDate;
            BOtemp.ShipDate = DOTemp.ShipDate;
            BOtemp.DeliveryDate = DOTemp.DeliveryDate;
            if (DOTemp.ShipDate < DOTemp.DeliveryDate) BOtemp.OrderStatus = BO.Enums.Status.shiped;
            else if (DOTemp.ShipDate < DateTime.Today) BOtemp.OrderStatus = BO.Enums.Status.shiped;
            else BOtemp.OrderStatus = BO.Enums.Status.approved;
            BOtemp.Details = new();

            return BOtemp;
        }
        ///add product to Cart, returns updated cart
        public BO.Cart Add(BO.Cart boCart, int productId)
        {

            //create a copy of current cart to return it
            List<BO.Product> productList = new();
            foreach (DO.Product item in Dal.Product.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add") ) productList.Add(ConvertDo2BoProduct(item));

            List<BO.OrderItem> orderItemList = new();
            foreach (DO.OrderItem item in Dal.OrderItem.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add")) orderItemList.Add(ConvertDo2BoOrderItem(item));

            List<BO.Order> orderList = new();
            foreach (DO.Order item in Dal.Order.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add")) orderList.Add(ConvertDo2BoOrder(item));

            BO.Cart newBoCart = new();

            newBoCart.CustomerName = boCart.CustomerName;
            newBoCart.CustomerEmail = boCart.CustomerEmail;
            newBoCart.CustomeAdress = boCart.CustomeAdress;
            newBoCart.TotalPrice = boCart.TotalPrice;

            foreach (BO.OrderItem? item in boCart.Details)
                newBoCart.Details.Add(item);
            //end of copy cart - end stage 1

            //search for product if exist inside cart
            if (newBoCart.Details.Any(x => x?.ProductID == productId))
            {
                foreach (BO.Product item in productList)
                {
                    if (item.ID == productId || item.InStock > 0) //if product it in stock
                    {
                        item.InStock--;  //remove one from stock for orderItem
                        foreach (BO.OrderItem? orderItem in newBoCart.Details)  //search the order item with specific id
                        {
                            if (orderItem?.ProductID == productId)
                            {
                                orderItem.Amount++; //add one to amount in order item inside cart 
                                orderItem.TotalPrice += orderItem.ProductPrice; //update the total price in order item

                                newBoCart.TotalPrice += orderItem.ProductPrice; //update the total price in cart
                            }

                        }
                    }
                }
            }
            else //if product is not yet inside cart
            {
                foreach (BO.Product item in productList)
                {
                    if (item.ID == productId || item.InStock > 0) //if product it in stock
                    {
                        item.InStock--;  //remove one from stock for orderItem
                        BO.OrderItem? newOrderItem = new(); //create a new order item 
                        newOrderItem.ID = -987;
                        newOrderItem.ProductID = productId;
                        //newOrderItem.OrderID = 
                        newOrderItem.ProductName = item.Name;
                        newOrderItem.ProductPrice = item.Price;
                        newOrderItem.Amount = 1;
                        newOrderItem.TotalPrice = item.Price;

                        newBoCart.Details.Add(newOrderItem);  //a new order item to cart
                        newBoCart.TotalPrice += item.Price;   //update total price of cart
                    }
                }
            }
            return newBoCart;
        }
        ///updated the amount in the cart
        public BO.Cart UpdateAmount(BO.Cart boCart, int Id, int NewAmount)
        {
            BO.Cart newBoCart = new BO.Cart();
            newBoCart.CustomerName = boCart.CustomerName;
            newBoCart.CustomerEmail = boCart.CustomerEmail;
            newBoCart.CustomeAdress = boCart.CustomeAdress;
            newBoCart.Details = boCart.Details;
            newBoCart.TotalPrice = boCart.TotalPrice;

            foreach (BO.OrderItem item in boCart.Details)
            {
                if (item.ProductID == Id)
                {
                    if (NewAmount == item.Amount)
                    {
                        throw new BO.IdBOException("Amount has not changed");
                    }
                    else if (NewAmount == 0)
                    {
                        newBoCart.TotalPrice -= item.ProductPrice * item.Amount;
                        newBoCart.Details.Remove(item);
                        return newBoCart;
                    }
                    else if (NewAmount > item.Amount)
                    {
                        BO.OrderItem newOrderItem = item;
                        newBoCart.TotalPrice += (NewAmount - newOrderItem.Amount) * newOrderItem.ProductPrice;
                        newOrderItem.Amount = NewAmount;
                        newBoCart.Details.Remove(item);
                        newBoCart.Details.Add(newOrderItem);
                        return newBoCart;
                    }
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
            throw new BO.IdBOException("Item not found");
        }
        bool IdExistInProductList(int Id)
        {
            return (Dal.Product.GetAll().Any(x => x.Value.ID == Id));
        }
        ///Confirm the Cart and build objects of order
        public void ConfirmCart(BO.Cart boCart, string Name, string Email, string Addres)
        {
            foreach (BO.OrderItem? item in boCart.Details)
            {
                if (!IdExistInProductList(item?.ProductID ?? 0))
                {
                    throw new BO.IdBOException("not all the products in the cart are exist");
                }
                if (item?.Amount <= 0)
                {
                    throw new BO.IdBOException("negative Amount");
                }
                if (item?.Amount > Dal.Product.Get(x => x?.ID == item?.ProductID).Value.InStock)
                {
                    throw new BO.IdBOException("not enough in stock");
                }
                if (boCart.CustomerName == "" || boCart.CustomerName == null)
                {
                    throw new BO.IdBOException("Customer Name is not empty");
                }
                if (boCart.CustomeAdress == "" || boCart.CustomeAdress == null)
                {
                    throw new BO.IdBOException("Customer address is empty");
                }
                if (boCart.CustomerEmail == "" || boCart.CustomerEmail == null)
                {
                    throw new BO.IdBOException("Customer email is not valid");
                }
            }

            DO.Order newOrder = new DO.Order();
            newOrder.CustomerName = Name;
            newOrder.CustomeAdress = Addres;
            newOrder.CustomerEmail = Email;
            newOrder.OrderDate = DateTime.Now;

            newOrder.ID = Dal.Order.Add(newOrder);

            foreach (BO.OrderItem? item in boCart.Details)
            {
                DO.OrderItem? newOrderItem = ConvertBo2DoOrderItem(item);  //error
                Dal.OrderItem.Add(newOrderItem ?? throw new BO.nullObjectBOException("null"));
            }

        }
    }
}
/// interface of product items for manager and client
