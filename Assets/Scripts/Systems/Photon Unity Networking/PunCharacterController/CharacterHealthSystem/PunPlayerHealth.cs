using Photon.Pun;

namespace GDD.PUN
{
    public class PunPlayerHealth : PunCharacterHealth
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public virtual void UpdateEXPAndLevelPoint()
        {
            int[] amount = new[]
            {
                _characterSystem.GetUpdateEXP(),
                _characterSystem.GetMaxEXP(),
                _characterSystem.GetLevel()
            };
            
            photonView.RPC("PunRPCSetEXPAndLevel", RpcTarget.Others, amount);
        }
        
        [PunRPC]
        public virtual void PunRPCSetEXPAndLevel(int[] amount)
        {
            if(photonView.IsMine)
                return;
            
            _characterSystem.SetUpdateEXP(amount[0]);
            _characterSystem.SetEXP(amount[0]);
            _characterSystem.SetMaxEXP(amount[1]);
            _characterSystem.SetLevel(amount[2]);
            
            //print($"Level is : {amount[0]}");
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}