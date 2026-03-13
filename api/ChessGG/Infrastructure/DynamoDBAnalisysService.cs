using Amazon.DynamoDBv2.Model;

namespace ChessGG.Infrastructure;

using Domain;
using Application.Interfaces;

public class DynamoDBAnalysisService(DynamoDBClient client) : IAnalysisService
{
    public async Task<Analysis?> GetByPlayerAsync(string player)
    {
        await client.SetupAsync();
        
        var response = await client.Connection.QueryAsync(new QueryRequest {
            TableName = "Analysis",
            IndexName = "PlayerIndex",
            KeyConditionExpression = "Player = :player",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                { ":player", new AttributeValue { S = player } }
            }
        });

        var item = response.Items.FirstOrDefault();
        if (item is null)
            return null;
        
        var analisys = new Analysis {
            Id = Guid.Parse(item["Id"].S),
            Player = item["Player"].S,
            FinalsAbility = float.Parse(item["FinalsAbility"].N),
            OpeningTheory = float.Parse(item["OpeningTeory"].N),
            TaticalAttention = float.Parse(item["TaticalAttention"].N),
            ThreatAvaliation = float.Parse(item["ThreatAvaliation"].N),
            TimeManagement = float.Parse(item["TimeManagement"].N),
            RequestId = item["RequestId"].S
        };

        return analisys;
    }
    
    public async Task<Analysis> CreateEmptyAsync(string player)
    {
        await client.SetupAsync();

        var request = new PutItemRequest {
            TableName = "Analysis",
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

        var result = await client.Connection.PutItemAsync(request);
        if ((int)result.HttpStatusCode is < 200 or >= 300)
            throw new Exception("Failed to create analisys.");

        return new Analysis {
            Id = Guid.Parse(result.Attributes["Id"].S),
            FinalsAbility = 0,
            OpeningTheory = 0,
            TaticalAttention = 0,
            ThreatAvaliation = 0,
            TimeManagement = 0,
            RequestId = null,
            Player = player,
        };
    }

    public async Task UpdateAsync(Analysis analisys)
    {
        var request = new UpdateItemRequest {
            TableName = "Analysis",
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
                { ":opening", new AttributeValue { N = analisys.OpeningTheory.ToString() } },
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