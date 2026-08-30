using RimWorld;
using Verse;

namespace DivisionOfLabor
{
    /// <summary>
    /// Turns on the Work tab's manual priorities for new colonies.
    /// <para>
    /// This mod splits work into more, narrower jobs, which is only worth doing
    /// if you can order them - so manual priorities is the sane starting state.
    /// </para>
    /// <para>
    /// <see cref="GameComponent.StartedNewGame"/> fires only when a new game
    /// begins, never on load, so an existing colony keeps whatever the player
    /// chose. It is still only a default: the checkbox in the Work tab works as
    /// normal, and turning it off sticks.
    /// </para>
    /// </summary>
    public class WorkPriorityDefault : GameComponent
    {
        // RimWorld instantiates game components with the owning Game.
        public WorkPriorityDefault(Game game)
        {
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();

            PlaySettings? settings = Current.Game?.playSettings;
            if (settings != null)
            {
                settings.useWorkPriorities = true;
            }
        }
    }
}
