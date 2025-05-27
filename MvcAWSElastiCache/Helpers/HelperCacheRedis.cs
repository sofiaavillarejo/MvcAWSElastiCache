using StackExchange.Redis;

namespace MvcAWSElastiCache.Helpers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer>
        CreateConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string connectionString = @"cache-coches.bf3mgs.ng.0001.use1.cache.amazonaws.com:6379";
            return ConnectionMultiplexer.Connect(connectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }

    }
}
