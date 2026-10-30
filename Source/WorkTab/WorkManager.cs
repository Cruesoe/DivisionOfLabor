using Verse;

namespace DivisionOfLabor.WorkTab
{
    /// <summary>
    /// Work Manager draws a 16px icon above each work type header; the labels move up to leave room for it.
    /// </summary>
    internal static class WorkManager
    {
        private static readonly bool Active = ModsConfig.IsActive("LordKuper.WorkManager");

        public static float HeaderOffset => Active ? 16f + GenUI.GapTiny : 0f;
    }
}
