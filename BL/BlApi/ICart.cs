using BO;
namespace BlApi
{
    public interface ICart
    {
        ///add product to Cart, returns updated cart
        public Cart Add(Cart boCart, int Id);

        ///updated the amount in the cart
        public Cart UpdateAmount(Cart boCart, int Id, int NewAmount);

        ///Confirm the Cart and build objects of order
        public void ConfirmCart(Cart boCart, string Name, string Email, string Addres);
    }
} /// interface of product items for manager and client
