namespace BO;
using DO;
using System.Collections.Generic;
using static BO.Enums;
public class Cart
{

    // data
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomeAdress { get; set; }

    public List<DO.OrderItem?> Details = new List<DO.OrderItem?>();
    public double TotalPrice { get; set; }

    // methods
    public override string ToString()
    {
        string s;
        s = " CustomerName:" + CustomerName + "CustomerEmail" + CustomerEmail + "CustomeAdress:" + CustomeAdress + "Details: ";
        foreach (var item in Details)
        {
            s += item.ToString();
            s += "\n";
        }
        s += "totalPrice: " + TotalPrice;

        return s;
    }
}

