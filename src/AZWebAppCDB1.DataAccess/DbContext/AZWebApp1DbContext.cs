using System;
using Performance.Testing.Common;
using Performance.Testing.DataAccess.Repositories.CDRepositories.Implementations;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace Performance.Testing.DataAccess.DbContext
{
    public sealed class AZWebApp1DbContext
    {
        private static AZWebApp1DbContext? _context;

        private static readonly Tuple<string, string>[] _tables = 
            new Tuple<string, string>[] { 
                Tuple.Create(UserRepository.CONTAINER_NAME, UserRepository.PARTITION_KEY),
                Tuple.Create(PostRepository.CONTAINER_NAME, PostRepository.PARTITION_KEY),
                Tuple.Create(CommentRepository.CONTAINER_NAME, CommentRepository.PARTITION_KEY) 
            };

        private AZWebApp1DbContext()
        {
        }

        public static AZWebApp1DbContext GetContext()
        {
            if (_context is null)
            {
                _context = new AZWebApp1DbContext();
            }
            return _context;
        }

        public string GetConnectionString(IConfiguration configuration)
        {
            return configuration[CosmosDbConstants.CONNECTION_STRING] ?? string.Empty;
        }


        public async void Migrate(IConfiguration configuration) {
            try
            {
                // Create CosmosClient
                CosmosClient _cosmosClient = new(configuration[CosmosDbConstants.CONNECTION_STRING]);
                var database = _cosmosClient.GetDatabase(configuration[CosmosDbConstants.DATABASE_NAME]);
                foreach (var table in _tables)
                {
                    _ = await database.CreateContainerIfNotExistsAsync(table.Item1, table.Item2 );
                }
                _cosmosClient.Dispose();
            }
            catch (Exception e) { System.Console.WriteLine(e.Message); }
        }
    }
}
