namespace BO;
using DO;
using System.Collections.Generic;
using static BO.Enums;
using static System.Formats.Asn1.AsnWriter;

public class BoOrderTracking
{

    ///data
    public int OrderID { get; set; }
    public Status Status { get; set; }
    public List<Tuple<DateTime, string>> TupleList = new List<Tuple<DateTime, string>>();

    ///// funcs
    //public override string ToString() => $@"
    //    OrderID: {OrderID}
    //    Category: {Status}
    //    list: {}

    //";

    public override string ToString()
    {
        
        Console.WriteLine("OrderID: ");
        Console.WriteLine(OrderID);
        Console.WriteLine("Category: ");
        Console.WriteLine(Status);
        Console.WriteLine("list: ");
        foreach (var score in TupleList)
        { Console.WriteLine(score.ToString()); }
    }


}




