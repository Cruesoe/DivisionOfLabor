using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DivisionOfLabor.WorkTab.Patches
{
    /// <summary>
    /// Draws Work tab headers in the chosen orientation, keeping vanilla sorting, clicking and tooltips.
    /// </summary>
    [HarmonyPatch(typeof(PawnColumnWorker_WorkPriority), nameof(PawnColumnWorker_WorkPriority.DoHeader))]
    public static class Patch_PawnColumnWorker_WorkPriority_DoHeader
    {
        private static readonly Action<PawnColumnWorker, Rect, PawnTable> HeaderClicked =
            AccessTools.MethodDelegate<Action<PawnColumnWorker, Rect, PawnTable>>(AccessTools.Method(typeof(PawnColumnWorker), "HeaderClicked"));

        private static readonly Func<PawnColumnWorker, PawnTable, string> GetHeaderTip =
            AccessTools.MethodDelegate<Func<PawnColumnWorker, PawnTable, string>>(AccessTools.Method(typeof(PawnColumnWorker), "GetHeaderTip"));

        public static bool Prefix(PawnColumnWorker_WorkPriority __instance, Rect rect, PawnTable table)
        {
            HeaderOrientation orientation = DivisionOfLaborSettings.HeaderOrientation;
            if (orientation == HeaderOrientation.Horizontal || !WorkTable.IsWorkTable(table))
            {
                return true;
            }

            rect.y -= WorkManager.HeaderOffset;
            bool vertical = orientation != HeaderOrientation.Inclined;

            if (table.SortingBy == __instance.def)
            {
                Texture2D tex = table.SortingDescending ? WorkTabTextures.SortingDescending : WorkTabTextures.Sorting;
                Rect sortingRect = vertical
                    ? new Rect(rect.xMax - tex.width - 1f, rect.yMax - tex.height - 1f, tex.width, tex.height)
                    : new Rect(rect.center.x - tex.width / 2f, rect.yMax - tex.height, tex.width, tex.height);
                GUI.DrawTexture(sortingRect, tex);
            }

            string label = __instance.def.workType.labelShort.CapitalizeFirst();
            Matrix4x4 originalMatrix = GUI.matrix;
            Rect labelRect;
            Matrix4x4 labelMatrix;
            switch (orientation)
            {
                case HeaderOrientation.Inclined:
                    (labelRect, labelMatrix) = LabelDrawer.DrawInclined(rect, label);
                    break;
                case HeaderOrientation.Vertical:
                    (labelRect, labelMatrix) = LabelDrawer.DrawVertical(rect, label);
                    break;
                default:
                    (labelRect, labelMatrix) = DrawStacked(rect, label);
                    break;
            }

            // Hit-test and highlight in the label's own rotated space.
            GUI.matrix = labelMatrix;
            bool mouseOver = labelRect.Contains(Event.current.mousePosition);
            if (Widgets.ButtonInvisible(labelRect))
            {
                HeaderClicked(__instance, rect, table);
            }
            if (mouseOver && !vertical)
            {
                Widgets.DrawHighlight(labelRect);
            }
            GUI.matrix = originalMatrix;

            if (mouseOver && vertical)
            {
                Widgets.DrawHighlight(rect);
            }
            MouseoverSounds.DoRegion(rect);
            if (mouseOver)
            {
                TooltipHandler.TipRegion(new Rect(0f, 0f, UI.screenWidth, UI.screenHeight), GetHeaderTip(__instance, table));
            }
            return false;
        }

        // Stacks up to four letters top to bottom, with a dot when the label is cut short.
        private static (Rect rect, Matrix4x4 matrix) DrawStacked(Rect rect, string label)
        {
            string stacked = string.Join("\n", label.Substring(0, Math.Min(4, label.Length)).ToCharArray());
            if (label.Length > 4)
            {
                stacked += ".";
            }

            TextAnchor anchor = Text.Anchor;
            GameFont font = Text.Font;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Vector2 size = HeaderCache.GetVerticalRotatedSize(stacked);
            Rect labelRect = new Rect(rect.center.x - size.x / 2f, rect.yMax - size.y, size.x, size.y);
            Widgets.Label(labelRect, stacked);
            Text.Anchor = anchor;
            Text.Font = font;
            return (labelRect, GUI.matrix);
        }
    }
}
