using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Linq.Expressions;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace AirmenMod
{
    public class ModEngine
    {
        public static DMTeamManager TeamManager;
        public static string PluginsFolder;
        public static List<Plugin> Plugins;
        public static string Version = "1.0";

        public static void Start(System.Object tm)
        {
            Console.WriteLine("Mod Engine has been started!");

            TeamManager = (DMTeamManager)tm;

            PluginsFolder = Application.dataPath + "/../plugins/";

            Console.WriteLine($"Mod Engine Plugins Folder: {PluginsFolder}");

            LoadPlugins();
            StartPlugins();
        }

        public static void LoadPlugin(string dir)
        {
            Console.WriteLine($"Plugin Checking {dir}");

            var settingsFile = Path.Combine(dir, "mod.json");

            Console.WriteLine($"Checking mod.json: {settingsFile}");

            if (File.Exists(settingsFile))
            {
                var settings = LoadSettings(settingsFile);

                if (settings != null)
                {
                    Console.WriteLine($"Loaded settings for: {settings.name}");

                    Plugins.Add(new Plugin()
                    {
                        DLLs = LoadDLLs(dir, settings.dlls),
                        Priority = settings.priority,
                        Path = dir,
                        Name = settings.name,
                        EntryFile = Path.Combine(dir, settings.pluginfile),
                        EntryClass = settings.pluginclass,
                        EntryNamespace = settings.pluginnamespace
                    });
                }
            }
            else
            {
                Console.WriteLine("No mod.json file file.");
            }
        }

        public static void LoadPlugins()
        {
            Plugins = new List<Plugin>();

            foreach (var dir in Directory.GetDirectories(PluginsFolder))
            {
                LoadPlugin(dir);
            }
        }

        public static bool StartPlugin(Plugin plugin)
        {
            Console.WriteLine($"Loading plugin: {plugin.Name} {plugin.EntryFile}");

            if (!File.Exists(plugin.EntryFile))
            {
                Console.WriteLine("Could not locate plugin's entry DLL.");
                return false;
            }

            try
            {
                plugin.DLL = Assembly.LoadFrom(plugin.EntryFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed loading plugin.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                return false;
            }

            plugin.EntryType = plugin.DLL.GetType($"{plugin.EntryClass}.{plugin.EntryClass}", false);

            if (plugin.EntryType == null)
            {
                Console.WriteLine($"Failed to find entry class {plugin.EntryClass}");
                return false;
            }

            try
            {
                plugin.Instance = Activator.CreateInstance(plugin.EntryType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed creating plugin.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                return false;
            }

            Console.WriteLine($"Plugin {plugin.Name} has been started!");
            return true;
        }
        
        public static void StartPlugins()
        {
            Plugins.Sort((x, y) => x.Priority.CompareTo(y.Priority));

            foreach (var plugin in Plugins)
            {
                var success = StartPlugin(plugin);
            }
        }

        public static bool OnPluginShutdown(Plugin plugin)
        {
            Console.WriteLine($"Shutting down plugin: {plugin.Name}");
            plugin.Running = false;

            try
            {
                var shutdown = plugin.EntryType.GetMethod("OnPluginShutdown");

                try
                {
                    shutdown.Invoke(plugin.Instance, new object[] { });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Plugin OnPluginShutdown() failed.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Plugin doesn't have OnPluginShutdown() function.");
            }

            Plugins.Remove(plugin);

            return true;
        }

        public static bool OnPluginReload(Plugin plugin)
        {
            Console.WriteLine($"Reloading plugin: {plugin.Name}");

            try
            {
                var reload = plugin.EntryType.GetMethod("OnPluginReload");

                try
                {
                    reload.Invoke(plugin.Instance, new object[] { });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Plugin OnPluginReload() failed.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Plugin doesn't have OnPluginReload() function.");
            }

            return true;
        }

        public static bool OnPluginStop(Plugin plugin)
        {
            Console.WriteLine($"Stopping plugin: {plugin.Name}");
            plugin.Running = false;

            try
            {
                var stop = plugin.EntryType.GetMethod("OnPluginStop");

                try
                {
                    stop.Invoke(plugin.Instance, new object[] { });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Plugin OnPluginStop() failed.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Plugin doesn't have OnPluginStop() function.");
            }

            return true;
        }
        public static bool OnPluginStart(Plugin plugin)
        {
            Console.WriteLine($"Starting plugin: {plugin.Name}");

            try
            {
                MethodInfo start = plugin.EntryType.GetMethod($"{plugin.EntryNamespace}.OnPluginStart");

                try
                {
                    start.Invoke(plugin.Instance, new object[] { });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Plugin OnPluginStart() failed.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Plugin doesn't have OnPluginStart() function.");
            }

            plugin.Running = true;

            return true;
        }

        public static Plugin GetPluginByName(string name)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.Name == name) return plugin;
            }

            return null;
        }

        public static List<Assembly> LoadDLLs(string dir, List<string> dlls)
        {
            List<Assembly> loadedDLLs = new List<Assembly>();

            foreach (var dll in dlls)
            {
                var path = Path.Combine(dir, dll);

                if (File.Exists(path) && path.ToLower().EndsWith(".dll"))
                {
                    Console.WriteLine($"Loading DLL: {path}");

                    try
                    {
                        loadedDLLs.Add(Assembly.LoadFrom(path));
                    } catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load DLL: {path}");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.Source);
                        Console.WriteLine(ex.StackTrace);
                    }
                } else
                {
                    Console.WriteLine($"Failed to find DLL: {dll}");
                }
            }

            return loadedDLLs;
        }

        public static ModSettings LoadSettings(string path)
        {
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<ModSettings>(json);
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Failed to read settings for {path}");
            }

            return null;
        }

        public static List<Dictionary<string, dynamic>> ToDict()
        {
            var plugins = new List<Dictionary<string, dynamic>>();

            foreach (var plugin in Plugins)
            {
                plugins.Add(plugin.ToDict());
            }

            return plugins;
        }
    }

    public class Plugin
    {
        public int Priority;
        public List<Assembly> DLLs;
        public string Path;
        public string Name;
        public string EntryFile;
        public string EntryClass;
        public dynamic Instance;
        public Assembly DLL;
        public Type EntryType;
        public string EntryNamespace;
        public bool Running;

        public Dictionary<string, dynamic> ToDict()
        {
            return new Dictionary<string, dynamic>()
            {
                { "priority", Priority },
                { "path", Path },
                { "name", Name },
                { "pluginfile", EntryFile },
                { "pluginclass", EntryClass },
                { "pluginnamespace", EntryNamespace },
                { "running", Running }
            };
        }
    }

    public class ModSettings
    {
        public string name = "No Name";
        public List<string> dlls = new List<string>();
        public int priority = 1;
        public string pluginfile = "plugin.dll";
        public string pluginclass = "Plugin";
        public string pluginnamespace = "Plugin";
    }
}