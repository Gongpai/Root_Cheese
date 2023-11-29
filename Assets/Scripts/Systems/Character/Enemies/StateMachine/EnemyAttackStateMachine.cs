using GDD.StrategyPattern;

namespace GDD.StateMachine
{
    public class EnemyAttackStateMachine : EnemyStateMachine
    {
        public override string StateName()
        {
            return "EnemyAttackState";
        }
    }
}