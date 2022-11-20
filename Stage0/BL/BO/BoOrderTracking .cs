namespace BO;
using DO;
using System.Collections.Generic;
using static BO.Enums;
public class BoOrderTracking
{
    ///constructor
    public BoOrderTracking(int OI, Category C, List<Tuple<DateTime, Status>> L)
    {
        OrderID = OI;
        Category = C;
        list = L;

    }

    ///data
    int OrderID { get; set; }
    Category Category { get; set; }
    List<Tuple<DateTime, Status>> list { get; set; }

    /// funcs
    public override string ToString() => $@"
        OrderID: {OrderID}
        Category: {Category}
        list: {list}
    ";

}




