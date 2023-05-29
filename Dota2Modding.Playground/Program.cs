using DBreeze.Utils;
using Dota2Modding.Common.Kv;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Searching;
using GameFinder.StoreHandlers.Steam;
using SteamDatabase.ValvePak;

Console.WriteLine("Hello, World!");

SteamHandler locator = new();
var dota = locator.FindOneGameById(570, out var _);
var dota2path = dota.Path;
var vpkDotaPath = Path.Combine(dota2path, "game", "dota", "pak01_dir.vpk");
var vpkDotaLvPath = Path.Combine(dota2path, "game", "dota_lv", "pak01_dir.vpk");

using var workspace = new Packages();

workspace.AddVpk(vpkDotaPath);
workspace.AddVpk(vpkDotaLvPath);

//var search = SearchEngine.Search<Entry>(trx).Block("hero").GetDocumentIDs();
//foreach (var id in search)
//{
//    var ent = trx.Select<byte[], byte[]>(typeof(Entry).FullName, 1.ToIndex(id)).ObjectGet<Entry>();
//    Console.WriteLine(ent);
//}

Console.WriteLine($"Indexed {workspace.Count} items");

foreach (var result in workspace.Find("npc"))
{
    Console.WriteLine(result.Name);
}


var csc = new CancellationTokenSource();
var path = @"H:\Steam\SteamApps\common\dota 2 beta\game\dota_addons\aghanim_zero\scripts\npc\npc_abilities_custom.txt";
var kv = await KvParser.Parse(path, csc.Token);

Console.WriteLine("Complete");