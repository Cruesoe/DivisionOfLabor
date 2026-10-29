using UnityEngine;
using Verse;

namespace DivisionOfLabor.WorkTab
{
    [StaticConstructorOnStartup]
    public static class WorkTabTextures
    {
        public static readonly Texture2D Inclined = ContentFinder<Texture2D>.Get("DivisionOfLabor/HeaderInclined");
        public static readonly Texture2D Vertical = ContentFinder<Texture2D>.Get("DivisionOfLabor/HeaderVertical");
        public static readonly Texture2D VerticalRotated = ContentFinder<Texture2D>.Get("DivisionOfLabor/HeaderVerticalRotated");
        public static readonly Texture2D Horizontal = ContentFinder<Texture2D>.Get("DivisionOfLabor/HeaderHorizontal");

        public static readonly Texture2D Sorting = ContentFinder<Texture2D>.Get("UI/Icons/Sorting");
        public static readonly Texture2D SortingDescending = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending");
    }
}
