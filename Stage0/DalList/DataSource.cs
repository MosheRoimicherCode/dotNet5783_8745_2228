using DO;
using System.Collections;
using System.Security.Cryptography;

namespace Dal;

internal static class DataSource
{
    static internal readonly RandomNumberGenerator RandomNum = RandomNumberGenerator.Create();
    static internal Product ProductArray = new Product[50];

}