﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace EmberCore.KernelServices.Plugins
{
    public class ResolverContext : AssemblyLoadContext, IResolverContext
    {
        private string FolderPath { get; }
        private ILogger Logger { get; }

        public string CurrentPath => FolderPath;

        public ResolverContext(string folder, ILogger logger)
        {
            FolderPath = folder;
            Logger = logger;
            if (!Directory.Exists(FolderPath))
            {
                throw new FileNotFoundException(FolderPath);
            }
        }

        public IEnumerable<Assembly> LoadAssemblies()
        {
            var pluginFiles = Directory.EnumerateFiles(FolderPath, "*.dll");
            foreach (var pluginPath in pluginFiles)
            {
                Assembly pluginAsm = default;
                try
                {
                    pluginAsm = LoadFromAssemblyPath(Path.Combine(FolderPath, pluginPath));
                }
                catch (Exception e)
                {
                    Logger.LogWarning(e, "Unknown plugin: {}", pluginPath);
                }
                if (pluginAsm != null) yield return pluginAsm;
            }
        }
    }
}
