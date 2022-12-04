using BO;
namespace BlApi
{
    public interface IBoCart
    {
        ///add product to Cart, returns updated cart
        public BoCart Add(BoCart boCart, int Id);

        ///updated the amount in the cart
        public BoCart UpdateAmount(BoCart boCart, int Id, int NewAmount);

        ///Confirm the Cart and build objects of order
        public void ConfirmCart(BoCart boCart, string Name, string Email, string Addres);
    }
} /// interface of product items for manager and client
