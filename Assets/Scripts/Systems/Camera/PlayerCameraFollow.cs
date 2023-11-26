using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject m_root_follow;
    [SerializeField] private bool m_canRotation = false;

    private Vector3 follow_pos;
    
    private void Update()
    {
        follow_pos = m_root_follow.transform.position;
        transform.position = follow_pos;

        if(m_canRotation)
            transform.rotation = m_root_follow.transform.rotation;
    }
}
