using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DivisionOfLabor.SearchAndRescue
{
    /// <summary>
    /// Search and Rescue reads a pawn's Field Rescue priority to decide who responds; it reads the more urgent of Doctor and Nurse instead.
    /// </summary>
    [HarmonyPatch]
    public static class Patch_Compatibility_WorkTypePriority
    {
        private static MethodBase? target;

        private static Func<Pawn, WorkTypeDef, int>? workTypePriority;

        public static bool Prepare()
        {
            target = FieldRescue.CompatibilityMethod("WorkTypePriority", typeof(Pawn), typeof(WorkTypeDef));
            if (target is MethodInfo method && workTypePriority == null)
            {
                workTypePriority = AccessTools.MethodDelegate<Func<Pawn, WorkTypeDef, int>>(method);
            }
            return target != null;
        }

        public static MethodBase TargetMethod()
        {
            return target!;
        }

        public static bool Prefix(Pawn __0, WorkTypeDef __1, ref int __result)
        {
            if (__1 == null || __1 != FieldRescue.Def || workTypePriority == null)
            {
                return true;
            }
            __result = FieldRescue.Best(workTypePriority(__0, WorkTypeDefOf.Doctor), workTypePriority(__0, FieldRescue.Nurse));
            return false;
        }
    }
}
