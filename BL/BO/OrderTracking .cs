namespace BO;
using DO;
using System.Collections.Generic;
using static BO.Enums;
using static System.Formats.Asn1.AsnWriter;

public class OrderTracking
{

    // data
    public int OrderID { get; set; }
    public Status? Status { get; set; }
    public List<Tuple<DateTime?, string?>?>? TupleList { get; set; }

    // methods
    //public override string ToString() => $@"
    //    OrderID: {OrderID}
    //    Category: {Status}
    //    list: {string.Join("\n", TupleList!)}
    //";

}




