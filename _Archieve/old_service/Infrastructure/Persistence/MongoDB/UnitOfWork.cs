using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDB;

public class UnitOfWork : IDisposable
{
    private readonly IMongoClient _client;
    private IClientSessionHandle _session;

    public UnitOfWork(MongoDbManager mongoDbManager)
    {
        _client = mongoDbManager.GetClient();
    }

    public void StartTransaction()
    {
        _session = _client.StartSession();
        _session.StartTransaction(new TransactionOptions(
            readConcern: ReadConcern.Majority,
            writeConcern: WriteConcern.WMajority
        ));
    }

    public void Commit()
    {
        if (_session.IsInTransaction)
        {
            _session.CommitTransaction();
        }
    }

    public void Rollback()
    {
        if (_session.IsInTransaction)
        {
            _session.AbortTransaction();
        }
    }

    public void Dispose()
    {
        _session?.Dispose();
    }
}