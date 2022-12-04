using System.Runtime.Serialization;

namespace DalApi

{

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
