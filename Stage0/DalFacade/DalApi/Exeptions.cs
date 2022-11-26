namespace DalApi

{
    public class IdException : Exception
    {
        static public string IdError = "not found id";
        public IdException(string str) { IdError = str; }
    }

    public class ProgramExit : Exception
    {
        static public string Exit = "Exit Program";
        public ProgramExit() {}
    }
    
}
