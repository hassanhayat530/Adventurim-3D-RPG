using Godot;
using System;
using System.Text;

public partial class LLMService : Node
{
    private HttpRequest _httpRequest;
    private const string ApiUrl = "https://api.anthropic.com/v1/messages";

    public struct ScenarioChoice
    {
        public string Id;
        public string Label;
    }

    public struct ScenarioData
    {
        public string Narrative;
        public System.Collections.Generic.List<ScenarioChoice> Choices;
    }

    public struct ScenarioEffect
    {
        public string Type;
        public string EnemyId;
        public string ItemId;
        public string Stat;
        public int Amount;
    }

    public struct ScenarioResolution
    {
        public string ResolutionText;
        public ScenarioEffect Effect;
    }

    public override void _Ready()
    {
        _httpRequest = GetNode<HttpRequest>("HttpRequest");

    }

    // Call this from anywhere: await LLMService instance's RequestScenario()
    public async System.Threading.Tasks.Task<ScenarioData> RequestScenario()
    {
        // --------------------------------------- Sending Request ---------------------------------------
        GD.Print("RequestScenario() called");
        string apiKey = System.Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
            throw new System.Exception("ANTHROPIC_API_KEY not set!");

        // prompt to put in the body of the request
        string prompt = @"
        You are generating a short interactive scenario for a fantasy RPG.
        Setting: a tense encounter in a market town.

        Respond with ONLY a raw JSON object, no markdown code fences, no explanation text before or after it.
        The JSON must exactly match this shape:
        {
        ""narrative"": ""2-4 sentences of scene description"",
        ""choices"": [
            {""id"": ""short_snake_case_id"", ""label"": ""short button text""}
        ]
        }
        Include 2 to 4 choices, each with a distinct tone (aggressive, diplomatic, cautious, curious, etc).
        ";

        // body of the json request
        var bodyDict = new Godot.Collections.Dictionary
        {
            { "model", "claude-sonnet-4-6" },
            { "max_tokens", 300 },
            { "messages", new Godot.Collections.Array
                {
                    new Godot.Collections.Dictionary
                    {
                        { "role", "user" },
                        { "content", prompt}
                    }
                }
            }
        };

        string jsonBody = Json.Stringify(bodyDict);

        string[] headers =
        [
            "Content-Type: application/json",
            "x-api-key: " + apiKey,
            "anthropic-version: 2023-06-01"
        ];

        Error err = _httpRequest.Request(ApiUrl, headers, HttpClient.Method.Post, jsonBody);
        if (err != Error.Ok)
            throw new System.Exception("Request failed to start: " + err);

        Variant[] result = await ToSignal(_httpRequest, HttpRequest.SignalName.RequestCompleted);

        // --------------------------------------- Parsing result ---------------------------------------
        long responseCode = result[1].AsInt64();
        byte[] rawBody = result[3].AsByteArray();

        // responseText is a string in form of an outer json that
        // consists of the actual message from the LLM which is
        // also a JSON (lets call it scenario JSON). It's path can look like
        // outerJSON/content/firstBlock/text (where text = scenarioJSON)
        string responseText = Encoding.UTF8.GetString(rawBody);

        if (responseCode != 200)
            throw new System.Exception("API error (" + responseCode + "): " + responseText);

        // processing outer JSON to recieve cleaned inner scenario JSON
        Json outerJson = new();
        Error outerParseError = outerJson.Parse(responseText);
        if (outerParseError != Error.Ok)
            throw new System.Exception("Outer JSON parse failed: " + outerJson.GetErrorMessage());

        Godot.Collections.Dictionary root = outerJson.Data.AsGodotDictionary();
        Godot.Collections.Array contentBlocks = root["content"].AsGodotArray();
        Godot.Collections.Dictionary firstBlock = contentBlocks[0].AsGodotDictionary();

        string rawText = firstBlock["text"].AsString();
        string cleanedText = CleanJsonResponse(rawText); // recieve clean Inner Json

        // processing inner scenario JSON to recieve scenario and choices
        Json scenarioJson = new();
        Error scenarioParseError = scenarioJson.Parse(cleanedText);
        if (scenarioParseError != Error.Ok)
            throw new System.Exception("Scenario JSON malformed: " + scenarioJson.GetErrorMessage() + " | Raw: " + cleanedText);

        Godot.Collections.Dictionary scenarioDict = scenarioJson.Data.AsGodotDictionary();
        ScenarioData scenario = new()
        {
            Narrative = scenarioDict["narrative"].AsString(),
            Choices = new System.Collections.Generic.List<ScenarioChoice>()
        };

        GD.Print("Narrative: " + scenario.Narrative);
        GD.Print("\nChoices:");

        foreach (Variant choiceVariant in scenarioDict["choices"].AsGodotArray())
        {
            Godot.Collections.Dictionary choiceDict = choiceVariant.AsGodotDictionary();
            scenario.Choices.Add(new ScenarioChoice
            {
                Id = choiceDict["id"].AsString(),
                Label = choiceDict["label"].AsString()
            });
            GD.Print(choiceDict["id"].AsString() + ": " + choiceDict["label"].AsString());
        }

        return scenario;
    }

    string CleanJsonResponse(string raw)
    {
        string trimmed = raw.Trim();
        if (trimmed.StartsWith("```"))
        {
            // Strip opening fence and closing fence
            int firstNewline = trimmed.IndexOf('\n');
            trimmed = trimmed.Substring(firstNewline + 1);
            int lastFence = trimmed.LastIndexOf("```");
            if (lastFence >= 0)
                trimmed = trimmed.Substring(0, lastFence);
        }
        return trimmed.Trim();
    }

    public async System.Threading.Tasks.Task TriggerScenario(Node parent, string scenarioScenePath = "res://Scenes/Scenario.tscn")
    {
        ScenarioData scenario;
        try { scenario = await RequestScenario(); }
        catch (System.Exception e)
        {
            GD.PrintErr("Scenario request failed, using fallback: " + e.Message);
            scenario = GetFallbackScenario();
        }

        var scenarioScene = (PackedScene)ResourceLoader.Load(scenarioScenePath);
        var instance = scenarioScene.Instantiate<global::Scenario>();
        instance.DisplayScenario(scenario);
        parent.AddChild(instance);
    }

    public ScenarioData GetFallbackScenario()
    {
        return new ScenarioData
        {
            Narrative = "The market is quiet today. Nothing unusual catches your eye.",
            Choices = new System.Collections.Generic.List<ScenarioChoice>
            {
                new() { Id = "continue", Label = "Continue on" }
            }
        };
    }

    public async System.Threading.Tasks.Task<ScenarioResolution> RequestResolution(ScenarioData originalScenario, string chosenChoiceId)
    {
        string apiKey = System.Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
            throw new System.Exception("ANTHROPIC_API_KEY not set!");

        ScenarioChoice chosen = originalScenario.Choices.Find(c => c.Id == chosenChoiceId);

        string prompt = @"
        You are resolving the outcome of a player's choice in a fantasy RPG scenario.

        Original scenario: " + originalScenario.Narrative + @"
        Player chose: """ + chosen.Label + @"""

        Respond with ONLY a raw JSON object, no markdown code fences, no explanation text before or after it.
        The JSON must exactly match this shape:
        {
          ""resolution_text"": ""1-2 sentences describing what happens as a result"",
          ""effect"": {
            ""type"": ""one of: combat, item_reward, stat_penalty, stat_reward, neutral"",
            ""enemy_id"": ""a short snake_case id if type is combat, otherwise null"",
            ""item_id"": ""a short snake_case id if type is item_reward, otherwise null"",
            ""stat"": ""a short snake_case stat name if type is stat_penalty or stat_reward, otherwise null"",
            ""amount"": ""an integer amount if type is stat_penalty or stat_reward, otherwise 0""
          }
        }
        ";

        var bodyDict = new Godot.Collections.Dictionary
        {
            { "model", "claude-sonnet-4-6" },
            { "max_tokens", 300 },
            { "messages", new Godot.Collections.Array
                {
                    new Godot.Collections.Dictionary
                    {
                        { "role", "user" },
                        { "content", prompt }
                    }
                }
            }
        };

        string jsonBody = Json.Stringify(bodyDict);
        string[] headers =
        {
            "Content-Type: application/json",
            "x-api-key: " + apiKey,
            "anthropic-version: 2023-06-01"
        };

        Error err = _httpRequest.Request(ApiUrl, headers, HttpClient.Method.Post, jsonBody);
        if (err != Error.Ok)
            throw new System.Exception("Request failed to start: " + err);

        Variant[] result = await ToSignal(_httpRequest, HttpRequest.SignalName.RequestCompleted);

        long responseCode = result[1].AsInt64();
        byte[] rawBody = result[3].AsByteArray();
        string responseText = Encoding.UTF8.GetString(rawBody);

        if (responseCode != 200)
            throw new System.Exception("API error (" + responseCode + "): " + responseText);

        Json outerJson = new();
        if (outerJson.Parse(responseText) != Error.Ok)
            throw new System.Exception("Outer JSON parse failed: " + outerJson.GetErrorMessage());

        Godot.Collections.Dictionary root = outerJson.Data.AsGodotDictionary();
        string rawText = root["content"].AsGodotArray()[0].AsGodotDictionary()["text"].AsString();
        string cleanedText = CleanJsonResponse(rawText);

        Json resolutionJson = new();
        if (resolutionJson.Parse(cleanedText) != Error.Ok)
            throw new System.Exception("Resolution JSON malformed: " + resolutionJson.GetErrorMessage() + " | Raw: " + cleanedText);

        Godot.Collections.Dictionary resolutionDict = resolutionJson.Data.AsGodotDictionary();
        Godot.Collections.Dictionary effectDict = resolutionDict["effect"].AsGodotDictionary();

        ScenarioResolution resolution = new()
        {
            ResolutionText = resolutionDict["resolution_text"].AsString(),
            Effect = new ScenarioEffect
            {
                Type = effectDict["type"].AsString(),
                EnemyId = effectDict["enemy_id"].VariantType == Variant.Type.Nil ? null : effectDict["enemy_id"].AsString(),
                ItemId = effectDict["item_id"].VariantType == Variant.Type.Nil ? null : effectDict["item_id"].AsString(),
                Stat = effectDict["stat"].VariantType == Variant.Type.Nil ? null : effectDict["stat"].AsString(),
                Amount = effectDict["amount"].AsInt32()
            }
        };

        return resolution;
    }
}

