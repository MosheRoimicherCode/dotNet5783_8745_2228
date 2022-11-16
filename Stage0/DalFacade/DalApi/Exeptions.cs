namespace DalApi

{
    public class IdException : Exception
    {
        static public string IdError;
        public IdException() { IdError = "Id not found"; }
    }
    
}
