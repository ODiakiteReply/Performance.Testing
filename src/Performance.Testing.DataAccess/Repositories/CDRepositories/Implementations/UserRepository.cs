using System;
using Microsoft.Azure.Cosmos;
using Performance.Testing.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Performance.Testing.DataAccess.Repositories.CDRepositories.Interfaces;

namespace Performance.Testing.DataAccess.Repositories.CDRepositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserRepository> _logger;
        public static readonly string CONTAINER_NAME = "Users";
        public static readonly string PARTITION_KEY = "/RoleId";

        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private CosmosClient GetConnection()
        {
            return new CosmosClient(
                _configuration[CosmosDbConstants.CONNECTION_STRING], 
                new CosmosClientOptions { ConnectionMode = ConnectionMode.Gateway });
        }

        private void DisposeConnection(CosmosClient client)
        {
            client.Dispose();
        }

        public async Task<Models.User> CreateAsync(Models.User user)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            user.Id = Guid.NewGuid().ToString();
            var result = await Task.FromResult(container.CreateItemAsync(user).Result.Resource);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<string?> DeleteAsync(string id, string partitionKey)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.DeleteItemAsync<Models.User>(id, new PartitionKey(partitionKey));
            var result = await Task.FromResult(item.Resource?.Id);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<Models.User?> GetByUsernameAsync(string username)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Models.User>(
                queryText: $"SELECT * FROM {CONTAINER_NAME} AS u WHERE u.Username = '{username}'").ReadNextAsync();
            var result = await Task.FromResult(item.Resource.FirstOrDefault());
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<Models.User?> GetByIdAsync(string id)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Models.User>(
                queryText: $"SELECT * FROM {CONTAINER_NAME} AS u WHERE u.id = '{id}'").ReadNextAsync();
            var result = await Task.FromResult(item.Resource.FirstOrDefault());
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<Models.User> UpdateAsync(Models.User user)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var result = await Task.FromResult(container.ReplaceItemAsync(user, user.Id).Result.Resource);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<IEnumerable<Models.User>> GetAllAsync()
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Models.User>(
                queryText: $"SELECT * FROM {CONTAINER_NAME}").ReadNextAsync();
            var results = await Task.FromResult(item.Resource);
            DisposeConnection(cosmosClient);
            return results;
        }
    }
}
