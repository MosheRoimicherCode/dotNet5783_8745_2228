using BlApi;
using Dal;
using DalApi;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        IDal Dal = new DalList();

        ///add product to Cart, returns updated cart
        public BO.Cart Add(BO.Cart boCart, int productId)
    {
        List<DO.Product?> productList = new();
        foreach (DO.Product? product in Dal.Product.GetAll())
            productList.Add(product ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));

        List<DO.OrderItem?> OrderItemList = new();
        foreach (DO.OrderItem? orderItem in Dal.OrderItem.GetAll())
            OrderItemList.Add(orderItem ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));

        List<DO.Order?> OrderList = new();
        foreach (DO.Order? order in Dal.Order.GetAll())
            OrderList.Add(order ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));

        BO.Cart newBoCart = new();
        newBoCart.CustomerName = boCart.CustomerName;
        newBoCart.CustomerEmail = boCart.CustomerEmail;
        newBoCart.CustomeAdress = boCart.CustomeAdress;
        foreach (DO.OrderItem? item in boCart.Details)
            newBoCart.Details.Add(item);
        newBoCart.TotalPrice = boCart.TotalPrice;

        ////search for product if exist inside cart
        //for (int i = 0; i < newBoCart.Details.Count; i++)
        //{
        //    if (newBoCart.Details[i]?.ProductID == productId)
        //    {
        //        foreach (DO.Product product in Dal.Product.GetAll())
        //        {
        //            if (product.ID == productId && product.InStock > 0)
        //            {
        //                DO.OrderItem newOrderItem = new()
        //                {
        //                    ID = newBoCart.Details[i].ID

        //                }
        //                newOrderItem.Amount++;
        //                newBoCart.Details.Remove(item);
        //                newBoCart.Details.Add(newOrderItem);
        //                newBoCart.TotalPrice += item.Price;
        //                return newBoCart;
        //            }
        //        }
        //    }
        //}
        //bool flag = false;
        //foreach (var productId in productList)
        //{
        //    if (productId.ID == Id && productId.InStock > 0)
        //    {
        //        flag = true;
        //        int ordId = 0;
        //        foreach (var o in OrderList)
        //        {
        //            if (o.CustomerName == boCart.CustomerName)
        //            {
        //                ordId = o.ID;
        //            }
        //        }
        //        DO.OrderItem newOrderItem = new()
        //        {
        //            ID = OrderItemList[OrderItemList.Count - 1].ID + 1,
        //            ProductID = productId.ID,
        //            OrderID = ordId,
        //            Price = productId.Price,
        //            Amount = 1
        //        };
        //        newBoCart.Details.Add(newOrderItem);
        //        newBoCart.TotalPrice += newOrderItem.Price;
        //    }
        //}
        //if (flag == false)
        //{
        //    throw new BO.IdBOException("product is not available");
        //}
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

        foreach (DO.OrderItem item in boCart.Details)
        {
            if (item.ProductID == Id)
            {
                if (NewAmount == item.Amount)
                {
                    throw new BO.IdBOException("Amount has not changed");
                }
                else if (NewAmount == 0)
                {
                    newBoCart.TotalPrice -= item.Price * item.Amount;
                    newBoCart.Details.Remove(item);
                    return newBoCart;
                }
                else if (NewAmount > item.Amount)
                {
                    DO.OrderItem newOrderItem = item;
                    newBoCart.TotalPrice += (NewAmount - newOrderItem.Amount) * newOrderItem.Price;
                    newOrderItem.Amount = NewAmount;
                    newBoCart.Details.Remove(item);
                    newBoCart.Details.Add(newOrderItem);
                    return newBoCart;
                }
                else if (NewAmount < item.Amount)
                {
                    DO.OrderItem newOrderItem = item;
                    newBoCart.TotalPrice -= (newOrderItem.Amount - NewAmount) * newOrderItem.Price;
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
        foreach (DO.Product? product in Dal.Product.GetAll())
            if (product?.ID == Id) return true;
        return false;
    }

        ///Confirm the Cart and build objects of order
        public void ConfirmCart(BO.Cart boCart, string Name, string Email, string Addres)
    {
        foreach (DO.OrderItem? item in boCart.Details)
        {
            if (!IdExistInProductList(item?.ProductID?? 0))
            {
                throw new BO.IdBOException("not all the products in the cart are exist");
            }
            if (item?.Amount <= 0)
            {
                throw new BO.IdBOException("negative Amount");
            }
            if (item?.Amount > Dal.Product.Get(item?.ProductID??0).InStock)
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

        foreach (DO.OrderItem? item in boCart.Details)
        {
            DO.OrderItem? newOrderItem = item;
            Dal.OrderItem.Add(newOrderItem?? throw new BO.nullObjectBOException("null"));
        }

    }
    }
}
/// interface of product items for manager and client
