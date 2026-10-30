using HarmonyLib;
using RimWorld;

namespace DivisionOfLabor.WorkTab.Patches
{
    /// <summary>
    /// Makes Work tab headers tall enough for the longest rotated label.
    /// </summary>
    [HarmonyPatch(typeof(PawnColumnWorker_WorkPriority), nameof(PawnColumnWorker_WorkPriority.GetMinHeaderHeight))]
    public static class Patch_PawnColumnWorker_WorkPriority_GetMinHeaderHeight
    {
        public static void Postfix(ref int __result, PawnTable table)
        {
            if (DivisionOfLaborSettings.HeaderOrientation == HeaderOrientation.Horizontal || !WorkTable.IsWorkTable(table))
            {
                return;
            }
            if (HeaderCache.MinHeaderHeight == 0)
            {
                HeaderCache.Recache(table);
            }
            __result = HeaderCache.MinHeaderHeight;
        }
    }
}
