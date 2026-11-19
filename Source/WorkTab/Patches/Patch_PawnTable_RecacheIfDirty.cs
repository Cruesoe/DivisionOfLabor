using HarmonyLib;
using RimWorld;

namespace DivisionOfLabor.WorkTab.Patches
{
    /// <summary>
    /// Recalculates the header height and priority range before the Work tab is rebuilt, so the rebuild sizes the headers with them.
    /// </summary>
    [HarmonyPatch(typeof(PawnTable), "RecacheIfDirty")]
    public static class Patch_PawnTable_RecacheIfDirty
    {
        public static void Prefix(PawnTable __instance, bool ___dirty)
        {
            if (___dirty && WorkTable.IsWorkTable(__instance))
            {
                HeaderCache.Recache(__instance);
            }
        }
    }
}
