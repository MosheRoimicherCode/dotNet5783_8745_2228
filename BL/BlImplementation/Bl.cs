using BlApi;

namespace BlImplementation
{
     internal sealed class Bl : IBl
    {
        public IProduct Product { get; }
        public IOrder Order { get; }
        public ICart Cart { get; }

        public Bl()
        {
            Product = new Product();
            Order = new Order();
            Cart = new Cart();
        }
    }
}
