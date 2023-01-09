using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class DalXml : IDal
    {
        public IProduct Product => throw new NotImplementedException();

        public IOrder Order => throw new NotImplementedException();

        public IOrderItem OrderItem => throw new NotImplementedException();
    }
}
