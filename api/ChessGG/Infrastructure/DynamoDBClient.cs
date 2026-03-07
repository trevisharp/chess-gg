using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

namespace ChessGG.Infrastructure;

public class DynamoDBClient(
    string serviceUrl, string region,
    string accessKey, string secretKey,
    bool deploy
    )
{
    bool needSetup = !deploy;
    readonly AmazonDynamoDBClient client = new (
        new BasicAWSCredentials(accessKey, secretKey),
        new AmazonDynamoDBConfig {
            ServiceURL = serviceUrl,
            AuthenticationRegion = region
        }
    );
    
    public AmazonDynamoDBClient Connection => client;

    public async Task SetupAsync()
    {
        if (!needSetup)
            return;
        needSetup = false;

        var tables = await client.ListTablesAsync();
        if (!tables.TableNames.Contains("Analysis"))
        {
            await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = "Analysis",
                AttributeDefinitions = [
                    new() { AttributeName = "Id", AttributeType = ScalarAttributeType.S },
                    new() { AttributeName = "Player", AttributeType = ScalarAttributeType.S }
                ],
                KeySchema = [
                    new KeySchemaElement { AttributeName = "Id", KeyType = KeyType.HASH }
                ],
                ProvisionedThroughput = new ProvisionedThroughput {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = [
                    new GlobalSecondaryIndex
                    {
                        IndexName = "PlayerIndex",
                        KeySchema = [
                            new KeySchemaElement { AttributeName = "Player", KeyType = KeyType.HASH }
                        ],
                        Projection = new Projection { ProjectionType = ProjectionType.ALL },
                        ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 5, WriteCapacityUnits = 5 }
                    }
                ]
            });
        }
        
        if (!tables.TableNames.Contains("Request"))
        {
            await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = "Request",
                AttributeDefinitions = [
                    new() { AttributeName = "Id", AttributeType = ScalarAttributeType.S },
                    new() { AttributeName = "Player", AttributeType = ScalarAttributeType.S }
                ],
                KeySchema = [
                    new KeySchemaElement { AttributeName = "Id", KeyType = KeyType.HASH }
                ],
                ProvisionedThroughput = new ProvisionedThroughput {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = [
                    new GlobalSecondaryIndex
                    {
                        IndexName = "PlayerIndex",
                        KeySchema = [
                            new KeySchemaElement { AttributeName = "Player", KeyType = KeyType.HASH }
                        ],
                        Projection = new Projection { ProjectionType = ProjectionType.ALL },
                        ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 5, WriteCapacityUnits = 5 }
                    }
                ]
            });
        }
    }
}