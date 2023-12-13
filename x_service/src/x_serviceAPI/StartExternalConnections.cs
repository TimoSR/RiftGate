using Infrastructure.Persistence._Interfaces;
using x_serviceAPI.Features._Registration;

namespace x_serviceAPI
{
    public class StartExternalConnections : IHostedService
    {
        private readonly IMongoDbManager _mongoDbManager;
        private readonly PubTopicsRegister _pubTopicsRegister;
        private readonly SubTopicsRegister _subTopicsRegister;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<StartExternalConnections> _logger;

        public StartExternalConnections(IServiceProvider serviceProvider, ILogger<StartExternalConnections> logger)
        {
            _mongoDbManager = serviceProvider.GetService<IMongoDbManager>();
            _pubTopicsRegister = serviceProvider.GetService<PubTopicsRegister>();
            _subTopicsRegister = serviceProvider.GetService<SubTopicsRegister>();
            _cacheManager = serviceProvider.GetService<ICacheManager>();
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

            if (_cacheManager == null)
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
