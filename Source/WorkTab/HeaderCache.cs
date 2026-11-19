using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace DivisionOfLabor.WorkTab
{
    /// <summary>
    /// Header height, priority range and per-column label text for the Work tab, rebuilt with the table.
    /// </summary>
    public static class HeaderCache
    {
        public const int MinPriority = 0;

        public static int MaxPriority = 4;
        public static int DefaultPriority = 3;
        public static int MinHeaderHeight;

        // Keyed by work type; rebuilt on every Work tab rebuild, so a language change or def reload replaces it.
        private static readonly Dictionary<WorkTypeDef, HeaderLabel> Labels = new Dictionary<WorkTypeDef, HeaderLabel>();

        public static HeaderLabel GetLabel(WorkTypeDef workType)
        {
            if (!Labels.TryGetValue(workType, out HeaderLabel? label))
            {
                label = Measure(workType);
                Labels[workType] = label;
            }
            return label;
        }

        public static void Recache(PawnTable table)
        {
            MaxPriority = PriorityMaster.MaxPriority ?? 4;
            DefaultPriority = PriorityMaster.DefaultPriority ?? 3;
            Labels.Clear();

            // Headers are as tall as the longest work type label is wide.
            float longest = 0f;
            foreach (PawnColumnDef column in table.Columns)
            {
                if (column.workerClass != typeof(PawnColumnWorker_WorkPriority))
                {
                    continue;
                }
                longest = Mathf.Max(longest, GetLabel(column.workType).Size.x + GenUI.GapTiny * 2f);
            }
            MinHeaderHeight = Mathf.CeilToInt(longest + GenUI.GapTiny);
        }

        // Labels are drawn in the small font, so they are measured in it.
        private static HeaderLabel Measure(WorkTypeDef workType)
        {
            GameFont font = Text.Font;
            Text.Font = GameFont.Small;

            string text = workType.labelShort.CapitalizeFirst();
            string stacked = string.Join("\n", text.Substring(0, Math.Min(4, text.Length)).ToCharArray());
            if (text.Length > 4)
            {
                stacked += ".";
            }
            HeaderLabel label = new HeaderLabel(text, Text.CalcSize(text), stacked, Text.CalcSize(stacked) * 1.1f);

            Text.Font = font;
            return label;
        }
    }
}
