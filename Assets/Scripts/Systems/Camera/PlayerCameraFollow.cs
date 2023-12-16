using System;
using System.Collections;
using System.Collections.Generic;
using GDD;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_root_follow;
    [SerializeField] private bool m_canRotation = false;

    public Transform root_follow
    {
        set => m_root_follow = value;
    }
    private Vector3 follow_pos;

    private void Start()
    {
        if (m_root_follow != null)
        {
            follow_pos = m_root_follow.position;
            gameObject.transform.position = follow_pos;
        }
    }

    private void Update()
    {
        if (m_root_follow != null)
        {
            gameObject.transform.position = follow_pos;
            follow_pos = m_root_follow.position;
        }

        if(m_canRotation)
            transform.rotation = m_root_follow.transform.rotation;
    }
}
