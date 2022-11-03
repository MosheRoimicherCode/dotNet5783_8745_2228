﻿using System.Text.Json.Serialization;
namespace DO;

public struct Order
{
    public Order(int I, string CN, string CE, string CA, DateTime OD, DateTime SD, DateTime DD)
    {
        ID = I; 
        CustomerName = CN; 
        CustomerEmail = CE; 
        CustomeAdress = CA; 
        OrderDate = OD; 
        ShipDate = SD; 
        DeliveryDate = DD;
    }
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    DateTime OrderDate { get; set; }
    DateTime ShipDate { get; set; }
    DateTime DeliveryDate { get; set; }

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

