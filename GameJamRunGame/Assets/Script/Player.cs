using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Vector3 m_Speed;

    private Rigidbody m_RigidBody;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_RigidBody.velocity = m_Speed;
    }
}
