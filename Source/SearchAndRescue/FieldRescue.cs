using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DivisionOfLabor.SearchAndRescue
{
    /// <summary>
    /// Search and Rescue's Field Rescue work type, folded into Doctor and Nurse; null when Search and Rescue isn't loaded. Resolved once per session.
    /// </summary>
    [StaticConstructorOnStartup]
    internal static class FieldRescue
    {
        public static readonly WorkTypeDef? Def = DefDatabase<WorkTypeDef>.GetNamedSilentFail("SAR_FieldRescue");

        public static readonly WorkTypeDef Nurse = DefDatabase<WorkTypeDef>.GetNamed("DOL_Nurse");

        /// <summary>
        /// Finds a static method on Search and Rescue's Compatibility class, or null when the mod or method is missing.
        /// </summary>
        public static MethodBase? CompatibilityMethod(string name, params System.Type[] parameters)
        {
            if (!ModsConfig.IsActive("CH4AcKO3.SearchAndRescue"))
            {
                return null;
            }
            System.Type? type = AccessTools.TypeByName("SearchAndRescue.Compatibility");
            MethodBase? method = type == null ? null : AccessTools.Method(type, name, parameters);
            if (method == null)
            {
                Log.WarningOnce("[Division of Labor] Search and Rescue's " + name + " wasn't found; Field Rescue priorities won't follow Doctor and Nurse.", name.GetHashCode());
            }
            return method;
        }

        /// <summary>
        /// The more urgent of two work priorities, where 0 is off.
        /// </summary>
        public static int Best(int a, int b)
        {
            if (a <= 0)
            {
                return b;
            }
            return b <= 0 ? a : System.Math.Min(a, b);
        }
    }
}
