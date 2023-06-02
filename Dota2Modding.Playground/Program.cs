using DBreeze.Utils;
using Dota2Modding.Common.Models;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using Dota2Modding.Common.Models.Searching;
using GameFinder.StoreHandlers.Steam;
using SteamDatabase.ValvePak;
using ValveKeyValue;
using ValveResourceFormat.IO;

Console.WriteLine("Hello, World!");

var csc = new CancellationTokenSource();
var path = @"H:\Steam\SteamApps\common\dota 2 beta\game\dota_addons\aghanim_zero\scripts\npc\npc_abilities_custom.txt";
var folder = Path.GetDirectoryName(path);
var exist = File.Exists(path);
var ss = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);

var kv = KvLoader.Plain.Deserialize(File.OpenRead(path), new()
{
    FileLoader = new AddonFileLoader(folder!),
});

Console.WriteLine("Complete");