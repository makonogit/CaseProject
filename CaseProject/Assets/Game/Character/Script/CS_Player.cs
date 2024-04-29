//------------------------------------
//�S���ҁF�����S
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------
//�v���C���[�N���X
//------------------------------------
public class CS_Player : MonoBehaviour
{
    private float m_fMovement = 0.0f;       //�ړ���
    private Vector3 m_v3DestinationPos;     //�ړ��ړI�n

    [SerializeField, Header("������TransForm")]
    private Transform m_tThisTrans;

    [SerializeField, Header("�ړ����x")]
    private float m_fMoveSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //m_v3DestinationPos = m_tThisTrans.position;

        //�I�u�W�F�N�g�̌��������߂�
        Vector3 Direction = m_v3DestinationPos - m_tThisTrans.position;

        //�ړI�n�ɓ�������܂ňړ�
        if (m_v3DestinationPos != m_tThisTrans.position && m_v3DestinationPos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_v3DestinationPos, m_fMoveSpeed * Time.deltaTime);
        }
        else
        {
            m_v3DestinationPos = Vector3.zero;
        }

    }

    //------------------------------------
    //�m�b�N�o�b�N�֐�
    //�����F�m�b�N�o�b�N�̕���,�m�b�N�o�b�N�̋���
    //------------------------------------
    public void KnockBack(Vector3 knockbackdirection,float knockbackpower)
    {
        //�m�b�N�o�b�N�̕����Ƌ������l�����ĖړI�n��ݒ�
        m_v3DestinationPos = m_tThisTrans.position - (knockbackdirection * knockbackpower);
    }

    //------------------------------------
    //�ړI�n�ݒ�֐�
    //�����F�ړI�n
    //------------------------------------
    private void SetDistination(Vector3 distination)
    {
        m_v3DestinationPos = distination;
    }


    //------------------------------------
    //�S�[���֐�
    //------------------------------------
    private void OnGoal()
    {
        //�A�j���[�V�����Đ�
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�S�[���ɓ���������S�[���C�x���g�̔���
        if(collision.transform.tag == "Goal")
        {
            OnGoal();
        }
    }

}
