using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace DivisionOfLabor.WorkTab.Patches
{
    /// <summary>
    /// Leaves room after the last Work tab column for the overhang of the inclined labels.
    /// </summary>
    [HarmonyPatch(typeof(PawnColumnWorker_RemainingSpace), nameof(PawnColumnWorker_RemainingSpace.GetMinWidth))]
    public static class Patch_PawnColumnWorker_RemainingSpace_GetMinWidth
    {
        public static bool Prefix(ref int __result, PawnTable table)
        {
            if (DivisionOfLaborSettings.HeaderOrientation != HeaderOrientation.Inclined || !WorkTable.IsWorkTable(table))
            {
                return true;
            }
            __result = Mathf.CeilToInt(HeaderCache.MinHeaderHeight * Mathf.Sqrt(3f) / 2f) / 2;
            return false;
        }
    }
}
