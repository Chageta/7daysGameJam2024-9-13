using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    Vector3 m_Speed;
    [SerializeField, Header("�����x")]
    Vector3 m_Acceleration;
    [SerializeField, Header("�����x")]
    Vector3 m_Deceleration;
    [SerializeField, Header("�ő呬�x")]
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
        // ���͎��̎擾
        InputGetAxis();

        // �ړ��i�����A�����A��~�j�̏���
        MoveVertical();

        // �E�����ɕ����]�����鏈��
        RotateHorizontal();
    }

    private void InputGetAxis()
    {
        m_MoveHorizontal = Input.GetAxis("Horizontal");
        m_MoveVertical = Input.GetAxis("Vertical");
    }

    private void MoveVertical()
    {
        // W�L�[�������Ă���Ƃ� (moveVertical > 0)
        if (m_MoveVertical > 0)
        {
            // MaxSpeed.z�܂ŉ���
            m_Speed.z += m_Acceleration.z * Time.deltaTime;
            m_Speed.z = Mathf.Clamp(m_Speed.z, 0, m_MaxSpeed.z);
        }
        // S�L�[�������Ă���Ƃ� (moveVertical < 0)
        else if (m_MoveVertical < 0)
        {
            // 0�܂Ō���
            m_Speed.z -= m_Deceleration.z * Time.deltaTime;
            m_Speed.z = Mathf.Clamp(m_Speed.z, 0, m_MaxSpeed.z);
        }
        // �ړ�����
        transform.position += transform.forward * m_Speed.z * Time.deltaTime;
    }

    private void RotateHorizontal()
    {
        KeyCode[] moveKeys = PlayerInput.Keys;
        if (Input.GetKeyDown(moveKeys[3]))
        {
            // �E�ɉ�]������
            transform.Rotate(0, 90, 0);
        }
        else if (Input.GetKeyDown(moveKeys[2]))
        {
            // ���ɉ�]������
            transform.Rotate(0, -90, 0);
        }
    }
}
