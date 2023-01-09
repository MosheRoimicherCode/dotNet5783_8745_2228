namespace Dal;
using DalApi;
using DO;

internal class DalProduct : IProduct
{
    public int Add(Product t)
    {
        throw new NotImplementedException();
    }

    public void Delete(int n)
    {
        throw new NotImplementedException();
    }

    public Product? Get(Func<Product?, bool> f)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Product t)
    {
        throw new NotImplementedException();
    }
}

