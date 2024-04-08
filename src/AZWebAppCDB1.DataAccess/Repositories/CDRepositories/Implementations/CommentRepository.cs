using System;
using AZWebAppCDB1.Models;
using AZWebAppCDB1.Common;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AZWebAppCDB1.DataAccess.Repositories.CDRepositories.Interfaces;


namespace AZWebAppCDB1.DataAccess.Repositories.CDRepositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<CommentRepository> _logger;
        public static readonly string CONTAINER_NAME = "Comments";
        public static readonly string PARTITION_KEY = "/PostId";

        public CommentRepository(IConfiguration configuration, ILogger<CommentRepository> logger)
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

        public async Task<Comment> CreateAsync(Comment comment)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            comment.Id = Guid.NewGuid().ToString();
            comment.CreatedAt = DateTime.Now;
            var result = await Task.FromResult(container.CreateItemAsync(comment).Result.Resource);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<string?> DeleteAsync(string id, string partitionKey)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.DeleteItemAsync<Comment>(id, new PartitionKey(partitionKey));
            var result = await Task.FromResult(item.Resource?.Id);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<Comment?> GetByIdAsync(string id)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Comment>(
                queryText: $"SELECT * FROM {CONTAINER_NAME} AS c WHERE c.id = '{id}'").ReadNextAsync();
            var result = await Task.FromResult(item.Resource.FirstOrDefault());
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<IEnumerable<Comment>> GetByPostAsync(string postId)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Comment>(
                queryText: $"SELECT * FROM {CONTAINER_NAME} AS c WHERE c.PostId = '{postId}'").ReadNextAsync();
            var results = await Task.FromResult(item.Resource);
            DisposeConnection(cosmosClient);
            return results;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var result = await Task.FromResult(container.ReplaceItemAsync(comment, comment.Id).Result.Resource);
            DisposeConnection(cosmosClient);
            return result;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            var cosmosClient = GetConnection();
            var db = cosmosClient.GetDatabase(
                _configuration[CosmosDbConstants.DATABASE_NAME]);
            var container = db.GetContainer(CONTAINER_NAME);
            var item = await container.GetItemQueryIterator<Comment>(
                queryText: $"SELECT * FROM {CONTAINER_NAME}").ReadNextAsync();
            var results = await Task.FromResult(item.Resource);
            DisposeConnection(cosmosClient);
            return results;
        }
    }
}
