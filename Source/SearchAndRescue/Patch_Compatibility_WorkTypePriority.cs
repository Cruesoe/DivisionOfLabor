using System.Reflection;
using HarmonyLib;
using Verse;

namespace DivisionOfLabor.SearchAndRescue
{
    /// <summary>
    /// Search and Rescue reads a pawn's Field Rescue priority to decide who responds; it reads Doctor's instead.
    /// </summary>
    [HarmonyPatch]
    public static class Patch_Compatibility_WorkTypePriority
    {
        private static MethodBase? target;

        public static bool Prepare()
        {
            target = FieldRescue.CompatibilityMethod("WorkTypePriority", typeof(Pawn), typeof(WorkTypeDef));
            return target != null;
        }

        public static MethodBase TargetMethod()
        {
            return target!;
        }

        public static void Prefix(ref WorkTypeDef __1)
        {
            FieldRescue.Redirect(ref __1);
        }
    }
}
