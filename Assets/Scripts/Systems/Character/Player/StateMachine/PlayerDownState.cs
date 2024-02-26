namespace GDD
{
    public class PlayerDownState : PlayerState
    {
        protected override void Start()
        {
            base.Start();
        }

        public override string StateName()
        {
            return "PlayerDownState";
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