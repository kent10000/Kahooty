using System.Net;
using System.Security.Principal;
using HtmlAgilityPack;
using Katoot;
using Newtonsoft.Json;

var client = new HttpClient();
client.BaseAddress = new Uri("https://kahoot.it/rest/kahoots/");

Console.WriteLine("Enter First Letters of Game UUID");
var id = Console.ReadLine();
Console.WriteLine("Now Enter Game Title");
string? game = null;

while (game == null)
{ 
    game = Console.ReadLine();
}

var uuid = await FindUuid(id, game);

if (uuid == default)
{
    Environment.Exit(1);
}

Console.WriteLine(uuid);

var req = await client.GetAsync(uuid.ToString());
var txt = await req.Content.ReadAsStringAsync();
var info = JsonConvert.DeserializeObject<Info>(txt);

Console.WriteLine(info.Title);
foreach (var question in info.Questions)
{
    //Console.WriteLine($"The Answer for {question.Layout} is...");
    var choiceCount = question.Choices.Length;
    var count = 0;
    foreach (var choice in question.Choices)
    {
        count++;
        if (choice.Correct) break;
    }
    Console.WriteLine(count);
    
}









async Task<Guid?> FindUuid(string? fChar, string title)
{
    if (client == null) throw new Exception("Client Not Instantiated");
    var request = await client.GetAsync("?query=" + title + "&limit=24");
    var raw = await request.Content.ReadAsStringAsync();
    var deserializeObject = JsonConvert.DeserializeObject<Search>(raw);
    if (deserializeObject == null) return default;
    
    Guid uuid = default;
    
    foreach (var entry in deserializeObject.entities)
    {
        var id = entry.card.uuid;
        //verbose
        //Console.WriteLine(id.ToString());
        //verbose end
        if (!id.ToString().Contains(fChar)) continue;
        Console.WriteLine("Game Found!");
        uuid = id;
        break;
    }
    return uuid;
}






