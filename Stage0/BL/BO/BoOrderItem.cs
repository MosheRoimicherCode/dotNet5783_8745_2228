﻿namespace BO;
public class BoOrderItem
{
    ///contructor
    public BoOrderItem(int PI, int OI, string PN, double PP, int A, double TP)
    {
        ProductID = PI;
        OrderID = OI;
        ProductName = PN;
        ProductPrice = PP; 
        Amount = A;
        TotalPrice = TP;
    }
    ///data
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public string ProductName { get; set; }
    public double ProductPrice { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }

    ///funcs
    public override string ToString() => $@"
        Product ID: {ProductID}, 
        OrderID: {OrderID}
    	TotalPrice: {TotalPrice}
    	Amount in stock: {Amount}";
}