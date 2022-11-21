using BlApi;
using Dal;
using DalApi;
using DO;

namespace BlImplementation
{
    internal class BoProduct : IBoProduct
    {
        IDal Dal = new DalList ();

        public void Add(BO.BoProduct item)
        {
            throw new NotImplementedException();
        }

        public BO.BoProduct Get(int Id)
        {
            throw new NotImplementedException();
        }

        public BO.BoProductItem Get(int Id, BO.BoCart cart)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(BO.BoProduct item)
        {
            throw new NotImplementedException();
        }

        public BO.BoProductForList GetLists()
        {
            BO.BoProductForList product = new BO.BoProductForList;
            return product;
        }
    }
}
