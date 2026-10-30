using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace DivisionOfLabor.WorkTab.Patches
{
    /// <summary>
    /// Scrolling over a priority box cycles the priority, or toggles the work type when manual priorities are off.
    /// </summary>
    [HarmonyPatch(typeof(PawnColumnWorker_WorkPriority), nameof(PawnColumnWorker_WorkPriority.DoCell))]
    public static class Patch_PawnColumnWorker_WorkPriority_DoCell
    {
        public static void Postfix(PawnColumnWorker_WorkPriority __instance, Rect rect, Pawn pawn)
        {
            Event current = Event.current;
            WorkTypeDef workType = __instance.def.workType;
            if (!DivisionOfLaborSettings.UseScrollWheel || current.type != EventType.ScrollWheel || current.delta.y == 0f
                || !Mouse.IsOver(rect) || pawn.WorkTypeIsDisabled(workType))
            {
                return;
            }

            bool wasActive = pawn.workSettings.WorkIsActive(workType);
            int priority = pawn.workSettings.GetPriority(workType);
            int newPriority;
            if (Find.PlaySettings.useWorkPriorities)
            {
                // Scrolling down raises the number; past the lowest priority it wraps to off, and back.
                newPriority = priority + (current.delta.y > 0f ? 1 : -1);
                if (newPriority > HeaderCache.MaxPriority)
                {
                    newPriority = HeaderCache.MinPriority;
                }
                else if (newPriority < HeaderCache.MinPriority)
                {
                    newPriority = HeaderCache.MaxPriority;
                }
            }
            else
            {
                newPriority = priority != HeaderCache.MinPriority ? HeaderCache.MinPriority : HeaderCache.DefaultPriority;
            }

            pawn.workSettings.SetPriority(workType, newPriority);
            SoundDefOf.DragSlider.PlayOneShotOnCamera();

            if (!wasActive && pawn.workSettings.WorkIsActive(workType) && workType.relevantSkills.Any()
                && pawn.skills.AverageOfRelevantSkillsFor(workType) <= 2f)
            {
                SoundDefOf.Crunch.PlayOneShotOnCamera();
            }
            current.Use();
        }
    }
}
