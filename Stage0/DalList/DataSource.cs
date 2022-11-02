using DO;
using System.Collections;
using System.Security.Cryptography;

namespace Dal;

internal static class DataSource
{
    static internal readonly RandomNumberGenerator _randomNum = RandomNumberGenerator.Create();
    static internal Product[] _productArr = new Product[50];
    static internal Order[] _orderArr = new Order[100];
    static internal OrderItem[] _orderItemArr = new OrderItem[200];
    static internal Array a = new Product[50];
    
}