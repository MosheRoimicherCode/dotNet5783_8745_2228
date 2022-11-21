using BO;
namespace BlApi
{
    public interface IBoProduct 
    {
        ///add product to data base - for manager user
        public void Add(BoProduct item);

        ///search for a product with specific Id for manager user 
        /// <returns> IBoProduct item </returns>
        public BoProduct Get(int Id);

        ///search for a product with specific Id inside client cart (for client user)
        /// <returns> BoProductItem </returns>
        public BoProductItem Get(int Id, BoCart cart);

        ///remove product from data base - for manager user
        public void Remove(int Id);

        ///update data for product in data base
        public void Update(BoProduct item);

        /// return a lost with all product in client order
        /// <returns> Product list </returns>
        public List<BO.BoProductForList> GetList();
    }
} /// interface of product items for manager and client
