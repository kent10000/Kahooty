#region

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Katoot;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;

#endregion

//TODO: CLEAN UP CODE
var sn = 0;
const bool old = true;
var mac = OperatingSystem.IsMacOS();
const bool human = true;
const int minMs = 100;
const int maxMs = 2500;

var client = new HttpClient();
client.BaseAddress = new Uri("https://kahoot.it/rest/kahoots/");
Console.WriteLine("Enter the username you wish to use");
string? username = null;
while (string.IsNullOrEmpty(username)) username = Console.ReadLine();

Console.WriteLine("Enter First Letters of Game UUID");
var id = Console.ReadLine();

Console.WriteLine("Enter the PIN");
string? pin = null;
while (string.IsNullOrEmpty(pin)) pin = Console.ReadLine();


var web = mac ? new FirefoxDriver(@"/Users/aaron/Documents") : new FirefoxDriver(@"C:\WebDriver\bin");


web.Navigate().GoToUrl("https://kahoot.it?pin=" + pin);
web.Manage().Timeouts().ImplicitWait =
    TimeSpan.FromMilliseconds(30000); //lower timespam = more resources but higher score
var p = web.FindElement(By.Name("nickname"));
var b = web.FindElement(By.TagName("button"));
p.SendKeys(username);
b.Click();


Console.WriteLine("Now Enter Game Title");
string? game = null;

while (string.IsNullOrEmpty(game)) game = Console.ReadLine();

var uuid = await FindUuid(id, game);

if (uuid == default) Environment.Exit(1);

Console.WriteLine(uuid);

var req = await client.GetAsync(uuid.ToString());
var txt = await req.Content.ReadAsStringAsync();
var info = JsonConvert.DeserializeObject<Info>(txt);

Console.WriteLine(info.Title);
var wait = new WebDriverWait(web, TimeSpan.FromSeconds(60));
var random = new Random();
var first = true;
var num = 0;
foreach (var question in info.Questions)
{
    num++;
    if (num < sn) continue;
    //var choices = wait.Until(e => e.FindElements(By.TagName("button"))).ToArray();


    //Console.WriteLine($"The Answer for {question.Layout} is...");

    //TODO: Humanize answers
    var count = 0;
    foreach (var choice in question.Choices)
    {
        count++;
        if (choice.Correct) break;
    }
    
    if (old)
    {
        wait.Until(x => x.FindElement(By.TagName("desc"))); //Broken On Chromium and webkit rn
    }
    else
    {
        wait.Until(x => x.FindElement(By.XPath("/html/body/div/div[1]/div/div/main/div[2]/div/div[2]"))); //Broken On Chromium and webkit rn
        Console.WriteLine("Found");
    }

    IWebElement[] choices;
    choices = old ? web.FindElements(By.TagName("button")).ToArray() : web.FindElements(By.XPath("/html/body/div/div[1]/div/div/main/div[2]/div/div[2]/button")).ToArray();
    
    
    Console.WriteLine("Clicking");
    if (human)
    {
        var ts = random.Next(minMs, maxMs);
        Thread.Sleep(ts);
    }
    try
    {
        var r = random.Next(0, 8);
        if (r != 3)
        {
            choices[count - 1].Click();
        }
        else
        {
            choices[0].Click();
        }
        
        
    }
    catch (IndexOutOfRangeException e)
    {
        continue;
    }

    var tile = count switch
    {
        1 => "Triangle",
        2 => "Diamond",
        3 => "Circle",
        _ => "Square"
    };

    Console.WriteLine($"Chose {tile}");
    Thread.Sleep(1000);
}


async Task<Guid?> FindUuid(string? fChar, string title)
{
    if (client == null) throw new Exception("Client Not Instantiated");
    var request = await client.GetAsync("?query=" + title + "&limit=24");
    var raw = await request.Content.ReadAsStringAsync();
    var deserializeObject = JsonConvert.DeserializeObject<Search>(raw);
    if (deserializeObject == null) return default;

    if (string.IsNullOrEmpty(fChar))
    {
        return deserializeObject.entities.First().card.uuid;
    }

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

bool Exists(By element)
{
    try { web.FindElement(element); }
    catch (NoSuchElementException) { return false; }

    return true;
}