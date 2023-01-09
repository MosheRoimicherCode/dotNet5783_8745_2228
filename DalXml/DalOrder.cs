namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DalOrder : IOrder
{
    public int Add(Order t)
    {
        throw new NotImplementedException();
    }

    public void Delete(int n)
    {
        throw new NotImplementedException();
    }

    public Order? Get(Func<Order?, bool> f)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(int n, Order t)
    {
        throw new NotImplementedException();
    }
}

