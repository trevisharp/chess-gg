using Amazon;
using Amazon.DynamoDBv2;

namespace ChessGG.Infrastructure;

public class DynamoDBClient(string serviceUrl, RegionEndpoint region)
{
    readonly AmazonDynamoDBClient client = new (new AmazonDynamoDBConfig {
        ServiceURL = serviceUrl,
        RegionEndpoint = region
    });
    
    public AmazonDynamoDBClient Connection => client;
}