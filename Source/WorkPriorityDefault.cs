using RimWorld;
using Verse;

namespace DivisionOfLabor
{
    /// <summary>
    /// Turns on the Work tab's manual priorities when a new game starts; loaded games keep their setting.
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
