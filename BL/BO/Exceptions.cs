﻿using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class IdBOException : Exception
    {
        public IdBOException() : base() { }
        public IdBOException(string message) : base(message) { }
        public IdBOException(string message, Exception innerException) : base(message, innerException) { }
        protected IdBOException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

    [Serializable]
    internal class nullObjectBOException : Exception
    {
        public nullObjectBOException() : base() { }
        public nullObjectBOException(string message) : base(message) { }
        public nullObjectBOException(string message, Exception innerException) : base(message, innerException) { }
        protected nullObjectBOException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }


    [Serializable]
    internal class ProductNameException : Exception
    {
        public ProductNameException() : base() { }
        public ProductNameException(string message) : base(message) { }
        public ProductNameException(string message, Exception innerException) : base(message, innerException) { }
        protected ProductNameException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

    [Serializable]
    internal class PriceException : Exception
    {
        public PriceException() : base() { }
        public PriceException(string message) : base(message) { }
        public PriceException(string message, Exception innerException) : base(message, innerException) { }
        protected PriceException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

    [Serializable]
    internal class InStockException : Exception
    {
        public InStockException() : base() { }
        public InStockException(string message) : base(message) { }
        public InStockException(string message, Exception innerException) : base(message, innerException) { }
        protected InStockException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

    [Serializable]
    internal class DeleteProductException : Exception
    {
        public DeleteProductException() : base() { }
        public DeleteProductException(string message) : base(message) { }
        public DeleteProductException(string message, Exception innerException) : base(message, innerException) { }
        protected DeleteProductException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

    [Serializable]
    internal class UpdateProductException : Exception
    {
        public UpdateProductException() : base() { }
        public UpdateProductException(string message) : base(message) { }
        public UpdateProductException(string message, Exception innerException) : base(message, innerException) { }
        protected UpdateProductException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }

}
