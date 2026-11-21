using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DivisionOfLabor.SearchAndRescue
{
    /// <summary>
    /// Search and Rescue falls back to the Field Rescue priority for each of its jobs; it falls back to the job's own work type (Doctor or Nurse) instead.
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

        public static void Prefix(WorkGiverDef __1, ref WorkTypeDef __2)
        {
            if (__2 == null || __2 != FieldRescue.Def)
            {
                return;
            }
            WorkTypeDef? own = __1?.workType;
            __2 = own != null && own != FieldRescue.Def ? own : WorkTypeDefOf.Doctor;
        }
    }
}
