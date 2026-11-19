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
            // Same pawns as the vanilla cell draws a box for.
            if (!DivisionOfLaborSettings.UseScrollWheel || current.type != EventType.ScrollWheel || current.delta.y == 0f
                || !Mouse.IsOver(rect) || pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork
                || pawn.WorkTypeIsDisabled(workType))
            {
                return;
            }

            bool manual = Find.PlaySettings.useWorkPriorities;
            bool wasActive = pawn.workSettings.WorkIsActive(workType);
            int priority = pawn.workSettings.GetPriority(workType);
            int newPriority;
            if (manual)
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
            bool nowActive = pawn.workSettings.WorkIsActive(workType);
            if (manual)
            {
                SoundDefOf.DragSlider.PlayOneShotOnCamera();
            }
            else
            {
                (nowActive ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff).PlayOneShotOnCamera();
            }

            // Turning a job on gets the same warnings as clicking the box.
            if (!wasActive && nowActive)
            {
                if (workType.relevantSkills.Any() && pawn.skills.AverageOfRelevantSkillsFor(workType) <= 2f)
                {
                    SoundDefOf.Crunch.PlayOneShotOnCamera();
                }
                if (pawn.Ideo != null && pawn.Ideo.IsWorkTypeConsideredDangerous(workType))
                {
                    Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(pawn, workType.gerundLabel), pawn, MessageTypeDefOf.CautionInput, false);
                    SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera();
                }
            }
            current.Use();
        }
    }
}
