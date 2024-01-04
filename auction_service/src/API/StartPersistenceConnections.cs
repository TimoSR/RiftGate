using API._DIRegister;
using CodingPatterns.InfrastructureLayer;
using Infrastructure.Persistence._Interfaces;

namespace API
{
    public class StartPersistenceConnections : IHostedService
    {
        private readonly IMongoDbManager _mongoDbManager;
        private readonly PubTopicsRegister _pubTopicsRegister;
        private readonly SubTopicsRegister _subTopicsRegister;
        private readonly ICache _cache;
        private readonly ILogger<StartPersistenceConnections> _logger;

        public StartPersistenceConnections(IServiceProvider serviceProvider, ILogger<StartPersistenceConnections> logger)
        {
            _mongoDbManager = serviceProvider.GetService<IMongoDbManager>();
            _pubTopicsRegister = serviceProvider.GetService<PubTopicsRegister>();
            _subTopicsRegister = serviceProvider.GetService<SubTopicsRegister>();
            _cache = serviceProvider.GetService<ICache>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_mongoDbManager == null)
            {
                _logger.LogWarning("MongoDB Manager is not configured.");
            }

            if (_pubTopicsRegister == null)
            {
                _logger.LogError("PubTopicsRegister is not available. This is critical.");
                // Handle this situation as needed; could even throw an exception to prevent the application from starting.
            }

            if (_subTopicsRegister == null)
            {
                _logger.LogError("SubTopicsRegister is not available. This is critical.");
                // Handle this situation as needed.
            }

            if (_cache == null)
            {
                _logger.LogWarning("CacheManager is not configured.");
            }

            // Actual start logic here.

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // There is nothing to do on stop, so just return a completed Task
            return Task.CompletedTask;
        }
    }
}
