namespace BO;
using System.Collections.Generic;

public class OrderTracking
{
    // data
    public int OrderID { get; set; }
    public Status? Status { get; set; }
    public List<Tuple<DateTime?, string?>?>? TupleList { get; set; }

   // methods
    public override string ToString() => $@"
        OrderID: {OrderID}
        Category: {Status}
        list: {string.Join("\n", TupleList!)}
    ";
}




