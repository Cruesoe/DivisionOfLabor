using UnityEngine;
using Verse;

namespace DivisionOfLabor.WorkTab
{
    [StaticConstructorOnStartup]
    public static class WorkTabTextures
    {
        public static readonly Texture2D Sorting = ContentFinder<Texture2D>.Get("UI/Icons/Sorting");
        public static readonly Texture2D SortingDescending = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending");
    }
}
