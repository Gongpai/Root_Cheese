using System;
using Photon.Pun;
using UnityEngine;

namespace GDD
{
    public class SetViewID : MonoBehaviour
    {
        [SerializeField] private PhotonView m_parent;
        private void Start()
        {
            if (m_parent == null)
                m_parent = gameObject.GetComponent<PhotonView>();
            
            GetComponent<TakeDamage>().SetViewID(m_parent.ViewID);
        }
    }
}