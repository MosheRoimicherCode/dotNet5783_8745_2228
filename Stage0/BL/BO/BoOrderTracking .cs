namespace BO;
using DO;
using System.Collections.Generic;
using static BO.Enums;
public class BoOrderTracking
{

    ///data
    int OrderID { get; set; }
    Category Category { get; set; }
    List<Tuple<DateTime, Status>> TupleList { get; set; }

    /// funcs
    public override string ToString() => $@"
        OrderID: {OrderID}
        Category: {Category}
        list: {TupleList}
    ";

}




