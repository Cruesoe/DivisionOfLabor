using System;
using System.Reflection;
using Verse;

namespace DivisionOfLabor.WorkTab
{
    /// <summary>
    /// Reads Priority Master's priority range by reflection; null when the mod is not active.
    /// </summary>
    internal static class PriorityMaster
    {
        private static readonly string[] PackageIds = { "Lauriichen.PriorityMod", "Lauriichan.PriorityMaster" };

        private static object? settings;

        private static object? Settings
        {
            get
            {
                if (settings != null || !Array.Exists(PackageIds, ModsConfig.IsActive))
                {
                    return settings;
                }
                Type? modType = GenTypes.GetTypeInAnyAssembly("PriorityMod.Core.PriorityMaster");
                if (modType != null)
                {
                    settings = modType.GetField("settings")?.GetValue(LoadedModManager.GetMod(modType));
                }
                return settings;
            }
        }

        public static int? MaxPriority => Call("GetMaxPriority");

        public static int? DefaultPriority => Call("GetDefPriority");

        private static int? Call(string methodName)
        {
            object? target = Settings;
            MethodInfo? method = target?.GetType().GetMethod(methodName);
            return (int?)method?.Invoke(target, null);
        }
    }
}
