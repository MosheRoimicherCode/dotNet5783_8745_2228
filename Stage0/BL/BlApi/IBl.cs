using System.Security.Principal;

namespace BlApi
{
    public interface IBl
    {
        public IBoCart BoCart { get; }
        public IBoOrder BoOrder { get; }
        public IBoProduct BoProduct { get; }
    }

}
