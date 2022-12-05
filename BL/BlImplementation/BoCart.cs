using BlApi;
using BO;
using Dal;
using DalApi;
using DO;

namespace BlImplementation
{
    internal class BoCart : IBoCart
    {
        IDal Dal = new DalList();

        ///add product to Cart, returns updated cart
        public BO.BoCart Add(BO.BoCart boCart, int Id)
        {
            List<DO.Product> productList = new();
            foreach (DO.Product? product in Dal.Product.GetAll())
                productList.Add(product?? throw new nullObjectBOException("null object.BoCart.Add"));
     
            List<DO.OrderItem> OrderItemList = new();
            foreach (DO.OrderItem? orderItem in Dal.OrderItem.GetAll())
                OrderItemList.Add(orderItem ?? throw new nullObjectBOException("null object.BoCart.Add"));
            
            List<DO.Order> OrderList = new();
            foreach (DO.Order? order in Dal.Order.GetAll())
                OrderList.Add(order ?? throw new nullObjectBOException("null object.BoCart.Add"));

            BO.BoCart newBoCart = new BO.BoCart();

            newBoCart.CustomerName = boCart.CustomerName;
            newBoCart.CustomerEmail = boCart.CustomerEmail;
            newBoCart.CustomeAdress = boCart.CustomeAdress;
            newBoCart.Details = boCart.Details;
            newBoCart.TotalPrice = boCart.TotalPrice;

            foreach (OrderItem item in  boCart.Details)
            {
                if (item.ProductID == Id)
                {
                    foreach (var p in productList)
                    {
                        if (p.ID == Id && p.InStock > 0)
                        {
                            DO.OrderItem newOrderItem = item;
                            newOrderItem.Amount = item.Amount + 1;
                            newBoCart.Details.Remove(item);
                            newBoCart.Details.Add(newOrderItem);
                            newBoCart.TotalPrice += item.Price;
                            return newBoCart;
                        }
                    }
                }
            }
            bool flag = false;
            foreach (var p in productList)
            {
                if (p.ID == Id && p.InStock > 0)
                {
                    flag = true;
                    int ordId = 0;
                    foreach (var o in OrderList)
                    {
                        if (o.CustomerName == boCart.CustomerName)
                        {
                            ordId = o.ID;
                        }
                    }
                    DO.OrderItem newOrderItem = new()
                    {
                        ID = OrderItemList[OrderItemList.Count - 1].ID + 1,
                        ProductID = p.ID,
                        OrderID = ordId,
                        Price = p.Price,
                        Amount = 1
                    };
                    newBoCart.Details.Add(newOrderItem);
                    newBoCart.TotalPrice += newOrderItem.Price;
                }
            }
            if (flag == false)
            {
                throw new BO.IdBOException("product is not available");
            } 
            return newBoCart;
        }

        ///updated the amount in the cart
        public BO.BoCart UpdateAmount(BO.BoCart boCart, int Id, int NewAmount)
        {
            BO.BoCart newBoCart = new BO.BoCart();
            newBoCart.CustomerName = boCart.CustomerName;
            newBoCart.CustomerEmail = boCart.CustomerEmail;
            newBoCart.CustomeAdress = boCart.CustomeAdress;
            newBoCart.Details = boCart.Details;
            newBoCart.TotalPrice = boCart.TotalPrice;

            foreach (OrderItem item in boCart.Details)
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
            {        
                if (product?.ID.Equals(Id) ?? throw new BO.nullObjectBOException("null object"))
                {
                    return true;
                }
            return false;
        }

        ///Confirm the Cart and build objects of order
        public void ConfirmCart(BO.BoCart boCart, string Name, string Email, string Addres)
        {
            foreach (OrderItem item in boCart.Details)
            {
                if (!IdExistInProductList(item.ProductID))
                {
                    throw new BO.IdBOException("not all the products in the cart are exist");
                }
                if (item.Amount <= 0)
                {
                    throw new BO.IdBOException("negative Amount");
                }
                if (item.Amount > Dal.Product.Get(item.ProductID).InStock) 
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

            foreach (OrderItem item in boCart.Details)
            {
                DO.OrderItem newOrderItem = item;
                Dal.OrderItem.Add(newOrderItem);
            }
            
        }
    }
} /// interface of product items for manager and client
