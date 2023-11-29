namespace GDD
{
    public class PlayerStateMachine : CharacterStateMachine<PlayerSystem>
    {
        protected override void Start()
        {
            base.Start();
        }

        public override string StateName()
        {
            return "PlayerState";
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