using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace DivisionOfLabor.WorkTab
{
    /// <summary>
    /// Header height and priority range for the Work tab, recalculated when the table is rebuilt.
    /// </summary>
    public static class HeaderCache
    {
        public const int MinPriority = 0;

        public static int MaxPriority = 4;
        public static int DefaultPriority = 3;
        public static int MinHeaderHeight;

        private static readonly Dictionary<string, Vector2> VerticalRotatedSizes = new Dictionary<string, Vector2>();

        public static Vector2 GetVerticalRotatedSize(string label)
        {
            if (!VerticalRotatedSizes.TryGetValue(label, out Vector2 size))
            {
                size = Text.CalcSize(label) * 1.1f;
                VerticalRotatedSizes[label] = size;
            }
            return size;
        }

        public static void Recache(PawnTable table)
        {
            MaxPriority = PriorityMaster.MaxPriority ?? 4;
            DefaultPriority = PriorityMaster.DefaultPriority ?? 3;

            // Headers are as tall as the longest work type label is wide.
            float longest = 0f;
            foreach (PawnColumnDef column in table.Columns)
            {
                if (column.workerClass != typeof(PawnColumnWorker_WorkPriority))
                {
                    continue;
                }
                float width = Text.CalcSize(column.workType.labelShort.CapitalizeFirst()).x + GenUI.GapTiny * 2f;
                longest = Mathf.Max(longest, width);
            }
            MinHeaderHeight = Mathf.CeilToInt(longest + GenUI.GapTiny);
        }
    }
}
