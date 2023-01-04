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
            //if (Product != null)
            Product = new Product();
            //if (Order != null)
            Order = new Order();
            //if(Cart != null)
            Cart = new Cart();
        }
    }
}
