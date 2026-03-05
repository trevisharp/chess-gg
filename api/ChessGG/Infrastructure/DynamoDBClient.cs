using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ChessGG.Infrastructure;

public class DynamoDBClient(string serviceUrl, RegionEndpoint region, bool deploy)
{
    bool needSetup = !deploy;
    readonly AmazonDynamoDBClient client = new (new AmazonDynamoDBConfig {
        ServiceURL = serviceUrl,
        RegionEndpoint = region
    });
    
    public AmazonDynamoDBClient Connection => client;

    public async Task SetupAsync()
    {
        if (needSetup)
            return;
        needSetup = false;
        
        var tables = await client.ListTablesAsync();
        if (!tables.TableNames.Contains("Analisys"))
        {
            await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = "Analisys",
                KeySchema = [
                    new KeySchemaElement { AttributeName = "Id", KeyType = KeyType.HASH }
                ],
                AttributeDefinitions = [
                    new() { AttributeName = "Player", AttributeType = ScalarAttributeType.S },
                    new() { AttributeName = "RatingId", AttributeType = ScalarAttributeType.S },
                    new() { AttributeName = "OpeningTeory", AttributeType = ScalarAttributeType.N },
                    new() { AttributeName = "ThreatAvaliation", AttributeType = ScalarAttributeType.N },
                    new() { AttributeName = "TaticalAttention", AttributeType = ScalarAttributeType.N },
                    new() { AttributeName = "TimeManagement", AttributeType = ScalarAttributeType.N },
                    new() { AttributeName = "FinalsAbility", AttributeType = ScalarAttributeType.N }
                ],
                ProvisionedThroughput = new ProvisionedThroughput {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            });
        }
        
        if (!tables.TableNames.Contains("Request"))
        {
            await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = "Request",
                KeySchema = [
                    new KeySchemaElement { AttributeName = "Id", KeyType = KeyType.HASH }
                ],
                AttributeDefinitions = [
                    new() { AttributeName = "Player", AttributeType = ScalarAttributeType.S },
                    new() { AttributeName = "Creation", AttributeType = ScalarAttributeType.S },
                    new() { AttributeName = "Status", AttributeType = ScalarAttributeType.S },
                    new() { AttributeName = "ProcessStatus", AttributeType = ScalarAttributeType.N }
                ],
                ProvisionedThroughput = new ProvisionedThroughput {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            });
        }
    }
}