using Photon.Pun;

namespace GDD.PUN
{
    public class PunUserNetLobbyControl : PunUserNetControl
    {
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            GetComponent<PlayerSystem>().idPhotonView = photonView.ViewID;

            if (photonView.IsMine)
            {
                gameObject.name += "[MasterClient]";
            }
            else
            {
                gameObject.name += $" [Other] [{photonView.ViewID}]";
            }
        }
    }
}