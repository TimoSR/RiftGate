using API.Features.UserManagerFeature.DomainLayer.Entities;
using API.Features.UserManagerFeature.DomainLayer.Enums;
using API.Features.UserManagerFeature.DomainLayer.Repositories;
using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using MongoDB.Driver;

namespace API.Features.UserManagerFeature.InfrastructureLayer.DomainRepositories
{
    public class UserRepository : MongoRepository<User>, IUserRepository
    {
        public UserRepository(
            IMongoDbManager dbManager, 
            ILogger<UserRepository> logger, 
            IDomainEventDispatcher eventDispatcher) : base(dbManager, eventDispatcher, logger)
        {
        }

        public async Task CreateEmailIndexesAsync()
        {
            try
            {
                var collection = GetCollection();
                var indexKeysDefinition = Builders<User>.IndexKeys.Ascending(u => u.Email);
                var indexOptions = new CreateIndexOptions { Unique = true }; // Assuming email should be unique
                var indexModel = new CreateIndexModel<User>(indexKeysDefinition, indexOptions);
                await collection.Indexes.CreateOneAsync(indexModel);
                _logger.LogInformation("Index created on 'email' field.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during index creation: {ex.Message}");
                throw;
            }
        }
        
        public async Task<bool> UpdateUserStatusByEmailAsync(string email, UserStatus newStatus)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<User>.Filter.Eq(u => u.Email, email);
                var update = Builders<User>.Update
                    .Set(u => u.Status, newStatus.ToString()) // If you have a field for tracking the last modification time
                    .Set(u => u.LastModified, DateTime.UtcNow);
                
                var updateResult = await collection.UpdateOneAsync(filter, update);

                return updateResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during updating status of user by email {email}: {ex.Message}");
                throw;
            }
        }
        
        public async Task<bool> DeleteUserByEmailAsync(string email)
        {
            try
            {
                var collection = GetCollection();
                var deleteResult = await collection.DeleteOneAsync(u => u.Email == email);

                if (deleteResult.DeletedCount > 0)
                {
                    _logger.LogInformation($"User with email {email} deleted successfully.");
                    return true;
                }
                else
                {
                    _logger.LogInformation($"No user found with email {email} to delete.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during deleting user by email {email}: {ex.Message}");
                throw;
            }
        }
        
        public Task<User> GetUserByIdAsync(string id)
        {
            return GetByIdAsync(id);
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<bool> CheckUserExistsAsync(string email)
        {
            var collection = GetCollection();
            var existingUser = await collection.Find(u => u.Email == email).FirstOrDefaultAsync();
            return existingUser != null;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var collection = GetCollection();
                return await collection.Find(u => u.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var collectionName = nameof(User) + "s";
                _logger.LogError($"Error retrieving user by email {email} from {collectionName}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CreateUserIfNotRegisteredAsync(User newUser)
        {
            using var session = await _dbManager.GetClient().StartSessionAsync();
            session.StartTransaction();
            try
            {
                // Use CheckUserExistsAsync to check if the user already exists
                if (await CheckUserExistsAsync(newUser.Email))
                {
                    // User already exists, no need to proceed further. Rollback any changes.
                    await session.AbortTransactionAsync();
                    return false;
                }

                var collection = GetCollection();
                await collection.InsertOneAsync(session, newUser);
                
                // Commit the transaction. If an error occurs during commit, it will throw an exception.
                await session.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                // If any exception occurs during the transaction, rollback changes.
                await session.AbortTransactionAsync();
                _logger.LogError($"Error during user registration: {ex.Message}");
                throw;
            }
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserEmailAsync(string userId, string newEmail)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserPasswordAsync(string userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            try
            {
                var collection = GetCollection();
                return await collection.Find(u => u.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var collectionName = nameof(User) + "s";
                _logger.LogError($"Error retrieving user by email {email} from {collectionName}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            return await DeleteAsync(id);
        }
    }
}