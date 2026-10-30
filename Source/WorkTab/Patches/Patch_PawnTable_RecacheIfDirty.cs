using HarmonyLib;
using RimWorld;

namespace DivisionOfLabor.WorkTab.Patches
{
    /// <summary>
    /// Recalculates the header height and priority range whenever the Work tab is rebuilt.
    /// </summary>
    [HarmonyPatch(typeof(PawnTable), "RecacheIfDirty")]
    public static class Patch_PawnTable_RecacheIfDirty
    {
        public static void Prefix(PawnTable __instance, bool ___dirty, out bool __state)
        {
            __state = ___dirty && WorkTable.IsWorkTable(__instance);
        }

        public static void Postfix(PawnTable __instance, bool __state)
        {
            if (__state)
            {
                HeaderCache.Recache(__instance);
            }
        }
    }
}
