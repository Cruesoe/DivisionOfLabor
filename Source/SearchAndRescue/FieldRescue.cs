using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DivisionOfLabor.SearchAndRescue
{
    /// <summary>
    /// Search and Rescue's Field Rescue work type, folded into Doctor; null when Search and Rescue isn't loaded. Resolved once per session.
    /// </summary>
    [StaticConstructorOnStartup]
    internal static class FieldRescue
    {
        public static readonly WorkTypeDef? Def = DefDatabase<WorkTypeDef>.GetNamedSilentFail("SAR_FieldRescue");

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
                Log.WarningOnce("[Division of Labor] Search and Rescue's " + name + " wasn't found; Field Rescue priorities won't follow Doctor.", name.GetHashCode());
            }
            return method;
        }

        /// <summary>
        /// Swaps Field Rescue for Doctor.
        /// </summary>
        public static void Redirect(ref WorkTypeDef workType)
        {
            if (workType != null && workType == Def)
            {
                workType = WorkTypeDefOf.Doctor;
            }
        }
    }
}
