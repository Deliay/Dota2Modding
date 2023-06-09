﻿using Dota2Modding.VisualEditor;
using Dota2Modding.VisualEditor.Configuration;
using Dota2Modding.VisualEditor.GUI.Abstraction.Document;
using Dota2Modding.VisualEditor.GUI.Abstraction.EditorMenu;
using EmberCore.Extensions;
using EmberCore.KernelServices.Plugins;
using EmberCore.KernelServices.UI.View;
using EmberKernel;
using EmberKernel.Services.UI.Extension;
using EmberWpfCore.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;
using System.Windows;

static string GetLoggerConsoleLogFormat(IConfigurationSection config)
{
    return config["ConsoleLogFormat"]
        ?? "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
}
static string GetFileLogFormat(IConfigurationSection config)
{
    return config["FileLogFormat"]
        ?? "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
}
static string GetLoggerFilePath(IConfigurationSection config)
{
    var loggerFolder = config["LogFolder"] ?? "logs";
    var loggerPath = Path.Combine(Directory.GetCurrentDirectory(), loggerFolder);
    if (!Directory.Exists(loggerPath)) Directory.CreateDirectory(loggerPath);
    var loggerFileName = config["LogFilePattern"] ?? "logs_.txt";
    return Path.Combine(loggerPath, loggerFileName);
}

await (new KernelBuilder()
    .UseConfiguration((cfg) =>
    {
        cfg.SetBasePath(Directory.GetCurrentDirectory());
        cfg.AddEnvironmentVariables();
        cfg.AddJsonFile("editor_config.json", optional: false, reloadOnChange: true);
    })
    .UsePluginOptions(Path.Combine(Directory.GetCurrentDirectory(), "editor_config.json"))
    .UseConfigurationModel<EditorConfiguration>()
    .UseLogger((ctx, logger) =>
    {
        var config = ctx.Configuration.GetSection("Logging");
        var loggerFile = GetLoggerFilePath(config);
        logger
        .AddConfiguration(config)
        .AddSerilog((builder) => builder
            .Enrich.FromLogContext()
            .WriteTo.Console(
                theme: SystemConsoleTheme.Colored,
                outputTemplate: GetLoggerConsoleLogFormat(config))
            .WriteTo.File(
                path: loggerFile,
                rollingInterval: RollingInterval.Day,
                outputTemplate: GetFileLogFormat(config))
            .WriteTo.Sink(LoggerSink.Instance))
        .AddDebug();
    })
    .UseEventBus()
    .UseKernelService<PluginResolver>()
    .UsePlugins<PluginsManager>()
    .UseWindowManager<EmberWpfUIService, Window>()
    .UseMvvmInterface(mvvm => mvvm.UseConfigurationModel())
    .UseKernelService<EditorMenuManager, IMenuItemManager>()
    .UseKernelService<DocumentManager>()
    .UseKernelService<RegisteredLayoutPanel>()
    .UseKernelService<RegisteredLayoutDocument>()
    .Build()
    .RunAsync());