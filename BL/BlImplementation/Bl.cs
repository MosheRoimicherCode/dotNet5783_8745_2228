using BlApi;
using BO;

namespace BlImplementation
{
    sealed public class Bl : IBl
    {
        public IBoProduct BoProduct => new BoProduct();
        public IBoOrder BoOrder => new BoOrder();
        public IBoCart BoCart => new BoCart();
    }
}
