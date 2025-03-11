using System;
using Performance.Testing.Common;
using Performance.Testing.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Performance.Testing.DataAccess.Repositories.CDRepositories.Interfaces;

namespace Performance.Testing.DataAccess.Repositories.CDRepositories.Implementations
{
    public class PostRepository : IPostRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostRepository> _logger;
        public static readonly string CONTAINER_NAME = "Posts";
        public static readonly string PARTITION_KEY = "/UserId";

        public PostRepository(IConfiguration configuration, ILogger<PostRepository> logger)
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

        public async Task<Post> CreateAsync(Post post)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            post.Id = Guid.NewGuid().ToString();
            post.CreatedAt = DateTime.UtcNow;
            var result = await Task.FromResult(container.CreateItemAsync(post).Result.Resource);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<string?> DeleteAsync(string id, string partitionKey)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.DeleteItemAsync<Post>(id, new PartitionKey(partitionKey));
            var result = await Task.FromResult(item.Resource?.Id);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<Post?> GetByIdAsync(string id)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Post>(
                queryText: $"SELECT * FROM {CONTAINER_NAME} AS p WHERE p.id = '{id}'").ReadNextAsync();
            var result = await Task.FromResult(item.Resource.FirstOrDefault());
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<IEnumerable<Post>> GetByUserAsync(string userId)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Post>(
                queryText: $"SELECT * FROM {CONTAINER_NAME} AS p WHERE p.UserId = '{userId}'").ReadNextAsync();
            var results = await Task.FromResult(item.Resource);
            DisposeConnection(cosmosClient);
            return results;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var result = await Task.FromResult(container.ReplaceItemAsync(post, post.Id).Result.Resource);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Post>(
                queryText: $"SELECT * FROM {CONTAINER_NAME}").ReadNextAsync();
            var results = await Task.FromResult(item.Resource);
            DisposeConnection(cosmosClient);
            return results;
        }
    }
}
