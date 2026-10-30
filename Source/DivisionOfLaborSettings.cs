using System.Collections.Generic;
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
        private const float RowHeight = 30f;
        private const float LabelWidthPct = 0.55f;
        private const float ResetWidth = 200f;

        private static readonly HeaderOrientation[] Orientations =
        {
            HeaderOrientation.Inclined,
            HeaderOrientation.Vertical,
            HeaderOrientation.VerticalRotated,
            HeaderOrientation.Horizontal
        };

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
            Text.Font = GameFont.Small;
            Rect resetRect = new Rect(inRect.xMax - ResetWidth, inRect.yMax - RowHeight, ResetWidth, RowHeight);
            Listing_Standard list = new Listing_Standard();
            list.Begin(new Rect(inRect.x, inRect.y, inRect.width, resetRect.y - inRect.y));

            Header(list, "DOL_SectionWorkTab".Translate());

            Rect style = LabeledRow(list, "DOL_HeaderOrientation".Translate(), "DOL_HeaderOrientationTip".Translate());
            if (Widgets.ButtonText(style.ContractedBy(0f, 2f), OrientationLabel(HeaderOrientation)))
            {
                Find.WindowStack.Add(new FloatMenu(OrientationOptions()));
            }
            DescriptionRow(list, ("DOL_Header" + HeaderOrientation + "Tip").Translate());

            Rect scroll = list.GetRect(RowHeight);
            Widgets.DrawHighlightIfMouseover(scroll);
            TooltipHandler.TipRegion(scroll, "DOL_UseScrollWheelTip".Translate());
            Widgets.CheckboxLabeled(scroll.ContractedBy(2f, 3f), "DOL_UseScrollWheel".Translate(), ref UseScrollWheel);

            list.End();

            if (Widgets.ButtonText(resetRect, "DOL_ResetAll".Translate()))
            {
                UseScrollWheel = true;
                HeaderOrientation = HeaderOrientation.Inclined;
            }
        }

        private static void Header(Listing_Standard list, string label)
        {
            Rect rect = list.GetRect(RowHeight);
            GUI.color = Widgets.SeparatorLabelColor;
            Text.Anchor = TextAnchor.LowerLeft;
            Widgets.Label(rect, label);
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Widgets.SeparatorLineColor;
            Widgets.DrawLineHorizontal(rect.x, rect.yMax, rect.width);
            GUI.color = Color.white;
            list.Gap(6f);
        }

        // Label on the left, control area on the right; the whole row highlights and shows the tooltip.
        private static Rect LabeledRow(Listing_Standard list, string label, string tip)
        {
            Rect row = list.GetRect(RowHeight);
            Widgets.DrawHighlightIfMouseover(row);
            TooltipHandler.TipRegion(row, tip);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(row.LeftPart(LabelWidthPct).ContractedBy(2f, 0f), label);
            Text.Anchor = TextAnchor.UpperLeft;
            return row.RightPart(1f - LabelWidthPct);
        }

        // Grey explanatory line under the row above it.
        private static void DescriptionRow(Listing_Standard list, string text)
        {
            float height = Text.CalcHeight(text, list.ColumnWidth - 4f);
            Rect rect = list.GetRect(height);
            GUI.color = Color.gray;
            Widgets.Label(new Rect(rect.x + 2f, rect.y, rect.width - 4f, rect.height), text);
            GUI.color = Color.white;
            list.Gap(6f);
        }

        private static List<FloatMenuOption> OrientationOptions()
        {
            List<FloatMenuOption> options = new List<FloatMenuOption>();
            foreach (HeaderOrientation orientation in Orientations)
            {
                options.Add(new FloatMenuOption(OrientationLabel(orientation), () => HeaderOrientation = orientation));
            }
            return options;
        }

        private static string OrientationLabel(HeaderOrientation orientation)
        {
            return ("DOL_Header" + orientation).Translate();
        }
    }
}
