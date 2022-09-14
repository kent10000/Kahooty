using System.Net;
using System.Security.Principal;
using HtmlAgilityPack;
using Katoot;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;

//gunna have to use selenium 

//or websocket
var client = new HttpClient();
client.BaseAddress = new Uri("https://kahoot.it/rest/kahoots/");
Console.WriteLine("Enter the username you wish to use");
string? username = null;
while (string.IsNullOrEmpty(username))
{
    username = Console.ReadLine();
}

Console.WriteLine("Enter First Letters of Game UUID");
var id = Console.ReadLine();

Console.WriteLine("Enter the PIN");
string? pin = null;
while (string.IsNullOrEmpty(pin))
{
    pin = Console.ReadLine();
}

var opt = new SafariOptions();
var web = new SafariDriver(opt);
web.Navigate().GoToUrl("https://kahoot.it?pin=" + pin);
web.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
var p = web.FindElement(By.Name("nickname"));
var b = web.FindElement(By.TagName("button"));
p.SendKeys(username);
b.Click();
Console.ReadKey();


Environment.Exit(1);


Console.WriteLine("Now Enter Game Title");
string? game = null;

while (string.IsNullOrEmpty(game))
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






