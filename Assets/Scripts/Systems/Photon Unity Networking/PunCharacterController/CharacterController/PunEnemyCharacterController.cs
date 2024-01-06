using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.PlayerLoop;

namespace GDD.PUN
{
    public class PunEnemyCharacterController : PunCharacterController
    {
        // System
        protected BulletFireManeuver _bulletFireManeuver;
        protected EnemySpawnBullet _enemySpawnBullet;
        
        protected override void Awake()
        {
            base.Awake();

            _bulletFireManeuver = GetComponent<BulletFireManeuver>();
            _enemySpawnBullet = _bulletFireManeuver.enemySpawnBullet;
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

        public void CallRaiseToggleFireEvent(BulletType type, int[] posIndex = default)
        {
            print("Fire To Online");
            
            object[] content;

            if (type != BulletType.Projectile)
            {
                content = new object[]
                {
                    photonView.ViewID,
                    type
                };
            }
            else
            {
                content = new object[]
                {
                    photonView.ViewID,
                    type,
                    posIndex
                };
            }

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };
            SendOptions sendOptions = new SendOptions()
            {
                Reliability = true,
                Encrypt = true
            };

            PhotonNetwork.RaiseEvent(_punEventCode, content, raiseEventOptions, sendOptions);
        }
        
        public override void OnEvent(EventData photonEvent)
        {
            base.OnEvent(photonEvent);
            
            byte eventCode = photonEvent.Code;
            
            if (eventCode == _punEventCode && !photonView.IsMine)
            {
                object[] datas = (object[])photonEvent.CustomData;
                
                //print("This View ID : " + photonView.ViewID + " :: Receive ID : " + (int)data[1]);
                if ((int)datas[0] == photonView.ViewID)
                {
                    if((BulletType)datas[1] != BulletType.Projectile)
                        _bulletFireManeuver.ToggleFire(_enemySpawnBullet);
                    else
                        _bulletFireManeuver.ToggleFire(_enemySpawnBullet, (int[])datas[2]);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}