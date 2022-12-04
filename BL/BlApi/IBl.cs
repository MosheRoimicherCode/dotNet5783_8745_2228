using System.Security.Principal;

namespace BlApi
{
    public interface IBl
    {
        public IBoProduct BoProduct { get; }
        public IBoCart BoCart { get; }
        public IBoOrder BoOrder { get; }
       
    }

}
