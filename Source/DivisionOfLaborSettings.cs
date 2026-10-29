using DivisionOfLabor.WorkTab;
using UnityEngine;
using Verse;

namespace DivisionOfLabor
{
    /// <summary>
    /// Work tab header orientation and scroll-wheel priorities.
    /// </summary>
    public class DivisionOfLaborSettings : ModSettings
    {
        public static bool UseScrollWheel = true;
        public static HeaderOrientation HeaderOrientation = HeaderOrientation.Inclined;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref UseScrollWheel, "UseScrollWheel", true);
            Scribe_Values.Look(ref HeaderOrientation, "HeaderOrientation", HeaderOrientation.Inclined);
            base.ExposeData();
        }

        public static void DoWindowContents(Rect inRect)
        {
            Rect scrollRow = new Rect(inRect.x, inRect.y, inRect.width, GenUI.ListSpacing);
            Widgets.CheckboxLabeled(scrollRow, "DOL_UseScrollWheel".Translate(), ref UseScrollWheel);
            TooltipHandler.TipRegion(scrollRow, "DOL_UseScrollWheelTip".Translate());

            Rect headerRow = new Rect(inRect.x, scrollRow.yMax + GenUI.Gap, inRect.width, GenUI.ListSpacing);
            Widgets.Label(headerRow, "DOL_HeaderOrientation".Translate());

            Rect optionRow = new Rect(inRect.x, headerRow.yMax, inRect.width, GenUI.ListSpacing);
            float quarter = inRect.width / 4f;
            DoOption(new Rect(inRect.x, optionRow.y, quarter, inRect.height), optionRow, HeaderOrientation.Inclined, WorkTabTextures.Inclined);
            DoOption(new Rect(inRect.x + quarter, optionRow.y, quarter, inRect.height), optionRow, HeaderOrientation.Vertical, WorkTabTextures.Vertical);
            DoOption(new Rect(inRect.x + quarter * 2f, optionRow.y, quarter, inRect.height), optionRow, HeaderOrientation.VerticalRotated, WorkTabTextures.VerticalRotated);
            DoOption(new Rect(inRect.x + quarter * 3f, optionRow.y, quarter, inRect.height), optionRow, HeaderOrientation.Horizontal, WorkTabTextures.Horizontal);
        }

        // Radio button with a preview picture below it; clicking either selects the orientation.
        private static void DoOption(Rect column, Rect row, HeaderOrientation orientation, Texture2D texture)
        {
            string key = "DOL_Header" + orientation;
            string label = key.Translate();
            float width = Text.CalcSize(label).x + Widgets.RadioButOnTex.width + GenUI.GapTiny;
            Rect radioRect = new Rect(column.center.x - width / 2f, row.y, width, row.height);
            if (Widgets.RadioButtonLabeled(radioRect, label, HeaderOrientation == orientation))
            {
                HeaderOrientation = orientation;
            }

            Rect textureRect = new Rect(column.center.x - texture.width / 2f, row.yMax + GenUI.Gap, texture.width, texture.height);
            if (Widgets.ButtonInvisible(textureRect))
            {
                HeaderOrientation = orientation;
            }
            GUI.DrawTexture(textureRect, texture, ScaleMode.ScaleToFit);
            if (HeaderOrientation == orientation)
            {
                Widgets.DrawBox(textureRect);
            }

            TooltipHandler.TipRegion(new Rect(column.x, row.y, column.width, textureRect.yMax - row.y), (key + "Tip").Translate());
        }
    }
}
