using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine.PlayerLoop;

namespace GDD.PUN
{
    public class PunEnemyCharacterController : PunCharacterController
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

        [PunRPC]
        public override void OnGetCharacterData()
        {
            base.OnGetCharacterData();
        }

        [PunRPC]
        public override void RPCInitializeCharacter(object[] datas, int OwnerNetID)
        {
            base.RPCInitializeCharacter(datas, OwnerNetID);
        }

        public override void OnEvent(EventData photonEvent)
        {
            base.OnEvent(photonEvent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}