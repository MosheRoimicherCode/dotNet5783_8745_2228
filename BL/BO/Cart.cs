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
    public List<BO.OrderItem?> Details = new List<BO.OrderItem?>();
    public double TotalPrice { get; set; }

    // methods
    public override string ToString() => $@"
        CustomerName: {CustomerName}
        CustomerEmail: {CustomerEmail}
        CustomeAdress: {CustomeAdress}
        Details: {string.Join("\n", Details!)}
        TotalPrice: {TotalPrice}
        ";
}

