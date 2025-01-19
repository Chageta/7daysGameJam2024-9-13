using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    Vector3 m_Speed;
    [SerializeField, Header("加速度")]
    Vector3 m_Acceleration;
    [SerializeField, Header("減速度")]
    Vector3 m_Deceleration;
    [SerializeField, Header("最大速度")]
    Vector3 m_MaxSpeed;

    private float m_MoveHorizontal;
    private float m_MoveVertical;

    private Rigidbody m_RigidBody;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 入力軸の取得
        InputGetAxis();

        // 移動（加速、減速、停止）の処理
        MoveVertical();

        // 右か左に方向転換する処理
        RotateHorizontal();
    }

    private void InputGetAxis()
    {
        m_MoveHorizontal = Input.GetAxis("Horizontal");
        m_MoveVertical = Input.GetAxis("Vertical");
    }

    private void MoveVertical()
    {
        // Wキーを押しているとき (moveVertical > 0)
        if (m_MoveVertical > 0)
        {
            // MaxSpeed.zまで加速
            m_Speed.z += m_Acceleration.z * Time.deltaTime;
            m_Speed.z = Mathf.Clamp(m_Speed.z, 0, m_MaxSpeed.z);
        }
        // Sキーを押しているとき (moveVertical < 0)
        else if (m_MoveVertical < 0)
        {
            // 0まで減速
            m_Speed.z -= m_Deceleration.z * Time.deltaTime;
            m_Speed.z = Mathf.Clamp(m_Speed.z, 0, m_MaxSpeed.z);
        }
        // 移動処理
        transform.position += transform.forward * m_Speed.z * Time.deltaTime;
    }

    private void RotateHorizontal()
    {
        KeyCode[] moveKeys = PlayerInput.Keys;
        if (Input.GetKeyDown(moveKeys[3]))
        {
            // 右に回転させる
            transform.Rotate(0, 90, 0);
        }
        else if (Input.GetKeyDown(moveKeys[2]))
        {
            // 左に回転させる
            transform.Rotate(0, -90, 0);
        }
    }
}
