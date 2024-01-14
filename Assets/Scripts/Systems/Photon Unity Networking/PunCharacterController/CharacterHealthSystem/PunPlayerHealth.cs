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
            
            if(!photonView.IsMine)
                return;
            UpdateEXPAndLevelPoint();
        }

        public virtual void UpdateEXPAndLevelPoint()
        {
            int[] amount = new[]
            {
                _characterSystem.GetEXP(),
                _characterSystem.GetMaxEXP(),
                _characterSystem.GetLevel()
            };
            
            photonView.RPC("PunRPCSetEXPAndLevel", RpcTarget.Others, amount);
        }
        
        [PunRPC]
        public virtual void PunRPCSetEXPAndLevel(int[] amount)
        {
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