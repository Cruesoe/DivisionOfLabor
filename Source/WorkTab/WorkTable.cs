using HarmonyLib;
using RimWorld;

namespace DivisionOfLabor.WorkTab
{
    public static class WorkTable
    {
        private static readonly AccessTools.FieldRef<PawnTable, PawnTableDef> TableDef = AccessTools.FieldRefAccess<PawnTable, PawnTableDef>("def");

        public static bool IsWorkTable(PawnTable table)
        {
            return TableDef(table) == PawnTableDefOf.Work;
        }
    }
}
