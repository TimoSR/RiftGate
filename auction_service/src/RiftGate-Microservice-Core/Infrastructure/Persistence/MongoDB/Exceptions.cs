namespace Infrastructure.Persistence.MongoDB
{
    public class MongoRepositoryException : Exception
    {
        public MongoRepositoryException(string message) : base(message) { }
        public MongoRepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class MongoRepositoryConnectionException : MongoRepositoryException
    {
        public MongoRepositoryConnectionException(string message) : base(message) { }
        public MongoRepositoryConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class MongoRepositoryDataValidationException : MongoRepositoryException
    {
        public MongoRepositoryDataValidationException(string message) : base(message) { }
        public MongoRepositoryDataValidationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class MongoRepositoryNotFoundException : MongoRepositoryException
    {
        public MongoRepositoryNotFoundException(string message) : base(message) { }
        public MongoRepositoryNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
