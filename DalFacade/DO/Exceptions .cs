using System.Runtime.Serialization;

namespace DO

{
    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
    }
    [Serializable]
    public class FactoryError : Exception
    {
        public FactoryError() : base() { }
        public FactoryError(string message) : base(message) { }
        public FactoryError(string message, Exception innerException) : base(message, innerException) { }
        protected FactoryError(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }
    [Serializable]
    public class IdException : Exception
    {
        public IdException() : base() { }
        public IdException(string message) : base(message) { }
        public IdException(string message, Exception innerException) : base(message, innerException) { }
        protected IdException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }
    [Serializable]
    public class ProgramExit : Exception
    {
        public ProgramExit() : base() { }
        public ProgramExit(string message) : base(message) { }
        public ProgramExit(string message, Exception innerException) : base(message, innerException) { }
        protected ProgramExit(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string ToString() => base.ToString();
    }
}
