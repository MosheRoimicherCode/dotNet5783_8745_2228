using BlImplementation;

namespace BlApi
{
    public class Factory
    {
        public static IBl Get()
        {
            IBl ibl = new Bl();
            return ibl;
        }
    }
}
