using Dota2Modding.Common.Models;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.KvTree;
using Dota2Modding.Common.Models.Parser;
using Dota2Modding.Common.Models.Project;
using Dota2Modding.Common.Models.Searching;
using GameFinder.StoreHandlers.Steam;
using Microsoft.Extensions.Logging;
using SteamDatabase.ValvePak;
using ValveKeyValue;
using ValveResourceFormat.IO;

Console.WriteLine("Hello, World!");

var dota2base = new SteamHandler().FindOneGameById(570, out var _).Path;
var path = Path.Combine(dota2base, @"game\dota_addons\aghanim_zero\addoninfo.txt");
var folder = Path.GetDirectoryName(path);
var folderName = Path.GetFileName(folder);
var exist = File.Exists(path);
var proj = new DotaProject(path, new Dota2Locator());
proj.InitBasePackages();
Console.WriteLine("Complete");