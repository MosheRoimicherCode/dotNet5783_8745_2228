using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class IdError : Exception
    {
        public IdError() : base() { }
        public IdError(string message) : base(message) { }
        public IdError(string message, Exception innerException) : base(message, innerException) { }
        protected IdError(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }


    [Serializable]
    internal class ProductNameError : Exception
    {
        public ProductNameError() : base() { }
        public ProductNameError(string message) : base(message) { }
        public ProductNameError(string message, Exception innerException) : base(message, innerException) { }
        protected ProductNameError(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

    [Serializable]
    internal class PriceError : Exception
    {
        public PriceError() : base() { }
        public PriceError(string message) : base(message) { }
        public PriceError(string message, Exception innerException) : base(message, innerException) { }
        protected PriceError(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

    [Serializable]
    internal class InStockError : Exception
    {
        public InStockError() : base() { }
        public InStockError(string message) : base(message) { }
        public InStockError(string message, Exception innerException) : base(message, innerException) { }
        protected InStockError(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

}
