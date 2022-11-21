namespace BO;
using DO;
using System.Collections.Generic;
using static BO.Enums;
public class BoOrderTracking
{

    ///data
    public int OrderID { get; set; }
    public Status Status { get; set; }
    public List<Tuple<DateTime, string>> TupleList { get; set; }

    /// funcs
    public override string ToString() => $@"
        OrderID: {OrderID}
        Category: {Status}
        list: {TupleList}
    ";

}




