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

    [SerializeField, Header("������Rigidbody")]
    private Rigidbody2D m_rThisRigidbody;

    [SerializeField, Header("�ړ����x")]
    private float m_fMoveSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (!m_tThisTrans) Debug.LogWarning("TransForm���ݒ肳��Ă��܂���");
        if (!m_rThisRigidbody) Debug.LogWarning("RigidBody���ݒ肳��Ă��܂���");
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
            //transform.Translate(0.0f, 0.1f, 0.0f);
        }
        else
        {
            //transform.Translate(0.0f, -0.1f, 0.0f);
            //m_rThisRigidbody.AddForce(0.0f,m_fMoveSpeed);
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
    //���̉e���ݒ�֐�
    //�����F�ړI�n
    //------------------------------------
    public void WindMove(CS_Wind.E_WINDDIRECTION distination,float windpower)
    {
        Vector3 Direction = Vector3.zero;

        //�����ɂ���ĕ�����ݒ�
        switch(distination)
        {
            case CS_Wind.E_WINDDIRECTION.NONE:
                Direction = Vector3.zero;
                break;
            case CS_Wind.E_WINDDIRECTION.LEFT:
                Direction = Vector3.right;
                break;
            case CS_Wind.E_WINDDIRECTION.RIGHT:
                Direction = Vector3.left;
                break;
            case CS_Wind.E_WINDDIRECTION.UP:
                Direction = Vector3.up;
                break;
        }

        //���̕����Ƌ������l�����ĖړI�n��ݒ�
        m_v3DestinationPos = m_tThisTrans.position + (Direction * windpower);
        Debug.Log(windpower);
        Debug.Log(m_v3DestinationPos);
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

        //���ɓ��������畗�̉e�����󂯂�
        //if(collision.transform.tag == "Wind")
        //{
        //    CS_Wind wind = transform.GetComponent<CS_Wind>();
        //    if (!wind) return;

        //    float power = wind.WindPower;
        //    Debug.Log(power);
        //    Vector3 Direction = m_tThisTrans.position - collision.transform.position;
        //    WindMove(Direction, power);
        //}

    }


    //private void OnTriggerStay2D(Collider2D collision)
    //{

    //    //���ɓ��������畗�̉e�����󂯂�
    //    if (collision.transform.tag == "Wind")
    //    {
    //        CS_Wind wind = transform.GetComponent<CS_Wind>();
    //        if (!wind) return;

    //        float power = wind.WindPower;
            
    //        Vector3 Direction = m_tThisTrans.position - collision.transform.position;
    //        WindMove(Direction, power);
    //    }

    //}

}
