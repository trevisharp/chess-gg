using Amazon.DynamoDBv2.Model;

namespace ChessGG.Infrastructure;

using Domain;
using Application.Interfaces;

public class DynamoDBAnalisysService(DynamoDBClient client) : IAnalisysService
{
    public async Task<Analisys?> GetByPlayerAsync(string player)
    {
        await client.SetupAsync();
        
        var response = await client.Connection.QueryAsync(new QueryRequest {
            TableName = "Analisys",
            KeyConditionExpression = "Player = :player",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                { ":player", new AttributeValue { S = player } }
            }
        });

        var item = response.Items.FirstOrDefault();
        if (item is null)
            return null;
        
        var analisys = new Analisys {
            Id = Guid.Parse(item["Id"].S),
            Player = item["Player"].S,
            FinalsAbility = float.Parse(item["FinalsAbility"].N),
            OpeningTeory = float.Parse(item["OpeningTeory"].N),
            TaticalAttention = float.Parse(item["TaticalAttention"].N),
            ThreatAvaliation = float.Parse(item["ThreatAvaliation"].N),
            TimeManagement = float.Parse(item["TimeManagement"].N),
            RequestId = item["RequestId"].S
        };

        return analisys;
    }
    
    public async Task CreateEmptyAsync(string player)
    {
        await client.SetupAsync();

        var request = new PutItemRequest {
            TableName = "Analisys",
            Item = new Dictionary<string, AttributeValue>
            {
                { "Player", new AttributeValue { S = player } },
                { "RequestId", new AttributeValue { S = null } },
                { "FinalsAbility", new AttributeValue { N = "0" } },
                { "OpeningTeory", new AttributeValue { N = "0" } },
                { "TaticalAttention", new AttributeValue { N = "0" } },
                { "ThreatAvaliation", new AttributeValue { N = "0" } },
                { "TimeManagement", new AttributeValue { N = "0" } }
            }
        };

        await client.Connection.PutItemAsync(request);
    }

    public async Task UpdateAsync(Analisys analisys)
    {
        var request = new UpdateItemRequest {
            TableName = "Analisys",
            Key = new Dictionary<string, AttributeValue> {
                { "Id", new AttributeValue { S = analisys.Id.ToString() } }
            },
            ExpressionAttributeNames = new Dictionary<string, string> {
                { "#RequestId", "RequestId" },
                { "#Player", "Player" },
                { "#OpeningTeory", "OpeningTeory" },
                { "#ThreatAvaliation", "ThreatAvaliation" },
                { "#TaticalAttention", "TaticalAttention" },
                { "#TimeManagement", "TimeManagement" },
                { "#FinalsAbility", "FinalsAbility" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":player", new AttributeValue { S = analisys.Player } },
                { ":requestid", new AttributeValue { S = analisys.RequestId } },
                { ":opening", new AttributeValue { N = analisys.OpeningTeory.ToString() } },
                { ":threat", new AttributeValue { N = analisys.ThreatAvaliation.ToString() } },
                { ":tactical", new AttributeValue { N = analisys.TaticalAttention.ToString() } },
                { ":time", new AttributeValue { N = analisys.TimeManagement.ToString() } },
                { ":finals", new AttributeValue { N = analisys.FinalsAbility.ToString() } }
            },
            UpdateExpression = 
                "SET #Player = :player, #OpeningTeory = :opening, #ThreatAvaliation = :threat, " +
                "#RequestId = :requestid, #TaticalAttention = :tactical, #TimeManagement = :time, #FinalsAbility = :finals"
        };

        await client.Connection.UpdateItemAsync(request);

    }
}