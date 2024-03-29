﻿namespace DO;

///A class to restore the witch products each client request
public struct OrderItem
{
    ///data
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }

    ///funcs
    public override string ToString() => $@"
        ID: {ID}, 
        Product ID: {ProductID}, 
        OrderID: {OrderID}
    	Price: {Price}
    	Amount of product in orderItem: {Amount}";

}
