﻿using System.Text.Json.Serialization;
namespace DO;

///A class to restore customer details
public struct Order
{
    ///constractor
    public Order(int I, string CN, string CE, string CA, DateTime OD, DateTime SD, DateTime DD)
    {
        int ID = I; 
        string CustomerName = CN; 
        string CustomerEmail = CE;
        string CustomeAdress = CA;
        DateTime OrderDate = OD;
        DateTime ShipDate = SD;
        DateTime DeliveryDate = DD;
    }

    ///data
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }

    /// funcs
    public override string ToString() => $@"
        Product ID:{ID}, 
        CustomerName: {CustomerName}
    	CustomerEmail: {CustomerEmail}
    	CustomeAdress: {CustomeAdress}
        OrderDate: {OrderDate}
        ShipDate: {ShipDate}
        DeliveryDate: {DeliveryDate}
    ";

}

