using UnityEngine;

namespace DivisionOfLabor.WorkTab
{
    /// <summary>
    /// A work type's header label, its stacked form and their sizes in the small font.
    /// </summary>
    public sealed class HeaderLabel
    {
        public readonly string Text;
        public readonly Vector2 Size;
        public readonly string Stacked;
        public readonly Vector2 StackedSize;

        public HeaderLabel(string text, Vector2 size, string stacked, Vector2 stackedSize)
        {
            Text = text;
            Size = size;
            Stacked = stacked;
            StackedSize = stackedSize;
        }
    }
}
