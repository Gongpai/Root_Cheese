namespace GDD
{
    public class PlayerMoveStateMachine : PlayerStateMachine
    {
        protected override void Start()
        {
            base.Start();
        }

        public override string StateName()
        {
            return "PlayerMoveState";
        }
        
        public override void OnStart(PlayerSystem contrller)
        {
            base.OnStart(contrller);
        }

        public override void Handle(PlayerSystem contrller)
        {
            base.Handle(contrller);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}