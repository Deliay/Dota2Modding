using Dota2Modding.Common.Models;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.KvTree;
using Dota2Modding.Common.Models.Parser;
using Dota2Modding.Common.Models.Project;
using Dota2Modding.Common.Models.Searching;
using GameFinder.StoreHandlers.Steam;
using Microsoft.Extensions.Logging;
using SteamDatabase.ValvePak;
using System.Text;
using ValveKeyValue;
using ValveResourceFormat.IO;

Console.WriteLine("Hello, World!");

var dota2base = new SteamHandler().FindOneGameById(570, out var _).Path;
var path = Path.Combine(dota2base, @"game\dota_addons\aghanim_zero\addoninfo.txt");
var proj = new DotaProject(path, new Dota2Locator());
proj.InitBasePackages();

var hero = proj.Heroes["npc_dota_hero_zero_saber"];

var entry = proj.Heroes.Mapping[hero];
using var stream = entry.LoadResourceStream(proj.Packages);

var kv = KvFile.Deserialize(stream);

hero.SoundSet = "Hero_Sven";
kv["npc_dota_hero_zero_saber"] = hero.Value;

Console.WriteLine(kv.Serialize());

Console.WriteLine("Complete"); 