using GDD.StrategyPattern;

namespace GDD
{
    public class SingleFireManeuver : EnemyManeuver
    {
        public override void Maneuver(EnemyStateMachine pawn)
        {
            base.Maneuver(pawn);
        }

        public override void Truce()
        {
            base.Truce();
        }
    }
}