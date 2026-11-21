using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DivisionOfLabor.SearchAndRescue
{
    /// <summary>
    /// Search and Rescue falls back to the Field Rescue priority for each of its jobs; it falls back to Doctor's instead.
    /// </summary>
    [HarmonyPatch]
    public static class Patch_Compatibility_DetailedWorkPriority
    {
        private static MethodBase? target;

        public static bool Prepare()
        {
            target = FieldRescue.CompatibilityMethod("DetailedWorkPriority", typeof(Pawn), typeof(WorkGiverDef), typeof(WorkTypeDef));
            return target != null;
        }

        public static MethodBase TargetMethod()
        {
            return target!;
        }

        public static void Prefix(ref WorkTypeDef __2)
        {
            FieldRescue.Redirect(ref __2);
        }
    }
}
