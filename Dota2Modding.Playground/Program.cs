using Dota2Modding.Common.Kv;

Console.WriteLine("Hello, World!");

var csc = new CancellationTokenSource();
var path = @"H:\Steam\SteamApps\common\dota 2 beta\game\dota_addons\aghanim_zero\scripts\npc\npc_abilities_custom.txt";
var kv = await KvParser.Parse(path, csc.Token);

Console.WriteLine("Complete");