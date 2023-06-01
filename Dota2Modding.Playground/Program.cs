﻿using DBreeze.Utils;
using Dota2Modding.Common.Models.GameStructure;
using Dota2Modding.Common.Models.Parser;
using Dota2Modding.Common.Models.Searching;
using GameFinder.StoreHandlers.Steam;
using SteamDatabase.ValvePak;

Console.WriteLine("Hello, World!");

var csc = new CancellationTokenSource();
var path = @"H:\Steam\SteamApps\common\dota 2 beta\game\dota_addons\aghanim_zero\scripts\npc\npc_abilities_custom.txt";
var exist = File.Exists(path);
var kv = KvLoader.Parse(path);

Console.WriteLine("Complete");