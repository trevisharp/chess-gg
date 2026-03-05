using System.Globalization;
using Amazon.DynamoDBv2.Model;

namespace ChessGG.Infrastructure;

using Domain;
using Application.Interfaces;

public class DynamoDBRequestService(DynamoDBClient client) : IRequestService
{
    public async Task<Request?> CreateAsync(string player)
    {
        await client.SetupAsync();

        var obj = new Request {
            Id = Guid.NewGuid(),
            Creation = DateTime.UtcNow,
            Player = player,
            ProcessStatus = 0,
            Status = RequestStatus.Waiting
        };

        var request = new PutItemRequest {
            TableName = "Request",
            Item = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { S = obj.Id.ToString() } },
                { "Creation", new AttributeValue { S = obj.Creation.ToString("o") } },
                { "Player", new AttributeValue { S = obj.Player } },
                { "ProcessStatus", new AttributeValue { N = obj.ProcessStatus.ToString() } },
                { "Status", new AttributeValue { S = obj.Status.ToString() } },
            }
        };

        await client.Connection.PutItemAsync(request);

        return obj;
    }

    public async Task<Request?> GetByIdAsync(string id)
    {
        await client.SetupAsync();
        
        var response = await client.Connection.QueryAsync(new QueryRequest {
            TableName = "Request",
            KeyConditionExpression = "Id = :id",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                { ":id", new AttributeValue { S = id } }
            }
        });

        var item = response.Items.FirstOrDefault();
        if (item is null)
            return null;
        
        var creation = DateTime.Parse(item["Creation"].S, 
            null, DateTimeStyles.RoundtripKind
        );

        var request = new Request {
            Id = Guid.Parse(item["Id"].S),
            Player = item["Player"].S,
            Creation = creation,
            ProcessStatus = float.Parse(item["ProcessStatus"].N),
            Status = Enum.Parse<RequestStatus>(item["Status"].S)
        };

        return request;
    }

    public async Task UpdateAsync(Request req)
    {
        var request = new UpdateItemRequest {
            TableName = "Analisys",
            Key = new Dictionary<string, AttributeValue> {
                { "Id", new AttributeValue { S = req.Id.ToString() } }
            },
            ExpressionAttributeNames = new Dictionary<string, string> {
                { "#Player", "Player" },
                { "#Creation", "Creation" },
                { "#Status", "Status" },
                { "#ProcessStatus", "ProcessStatus" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":player", new AttributeValue { S = req.Player } },
                { ":creation", new AttributeValue { S = req.Creation.ToString("o") } },
                { ":status", new AttributeValue { S = req.Status.ToString() } },
                { ":processstatus", new AttributeValue { N = req.ProcessStatus.ToString() } }
            },
            UpdateExpression =  "SET #Player = :player, #Creation = :creation, " + 
                "#Status = :status, #ProcessStatus = :processstatus"
        };

        await client.Connection.UpdateItemAsync(request);
    }
}