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

    private float m_fStartHeight;           //�J�n���̍���
    private bool m_IsStartFry = false;      //���V�J�n������
    
    [SerializeField, Header("������TransForm")]
    private Transform m_tThisTrans;

    [SerializeField, Header("������Rigidbody")]
    private Rigidbody2D m_rThisRigidbody;

    [SerializeField, Header("�V���E�X�̃A�j���[�^�[")]
    private Animator m_aThisAnimator;

    [SerializeField, Header("�ړ����x")]
    private float m_fMoveSpeed = 1.0f;




    //--------------------
    //�@��̓��������p
    //--------------------
    [SerializeField, Header("���TransForm")]
    private Transform m_tHandTrans;

    [SerializeField, Header("��̕��V��")]
    private float m_fHandFryLength = 0.1f;

    [SerializeField, Header("��̕��V�X�s�[�h")]
    private float m_fHandFrySpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_tThisTrans) Debug.LogWarning("�V���E�X��TransForm���ݒ肳��Ă��܂���");
        if (!m_rThisRigidbody) Debug.LogWarning("�V���E�X��RigidBody���ݒ肳��Ă��܂���");
        if (!m_tHandTrans) Debug.LogWarning("�V���E�X�̎��TransForm���ݒ肳��Ă��܂���");
        if (!m_aThisAnimator) Debug.LogWarning("�V���E�X��Animator���ݒ肳��Ă��܂���");

        m_fStartHeight = m_tThisTrans.position.y;   // �J�n���̍���
    }

    // Update is called once per frame
    void Update()
    {
        //���������痣���A�j���[�V�����̍Đ�
        if (m_fStartHeight < m_tThisTrans.position.y && !m_IsStartFry) 
        {
            m_IsStartFry = true;
            m_aThisAnimator.SetBool("TakeOff", true); 
        }

        
        //�肪�ӂ�ӂ퓮������
        m_tHandTrans.position = new Vector3(m_tHandTrans.position.x, m_tHandTrans.position.y + Wave, 0.0f);

    }

    //�ӂ�ӂ퓮���悤��Sin�g�Œl��Ԃ�
    private float Wave
    {
        get
        {
            return Mathf.Sin(Time.time * m_fHandFrySpeed) * m_fHandFryLength;
        }
    }

    //------------------------------------
    //�m�b�N�o�b�N�֐�
    //�����F�m�b�N�o�b�N�̕���,�m�b�N�o�b�N�̋���
    //------------------------------------
    public void KnockBack(Vector3 knockbackdirection,float knockbackpower)
    {
        //�m�b�N�o�b�N�̕����Ƌ������l�����ĖړI�n��ݒ�
        //m_v3DestinationPos = m_tThisTrans.position - (knockbackdirection * knockbackpower);

        //m_tThisTrans.position = m_v3DestinationPos;

        m_rThisRigidbody.AddForce(m_tThisTrans.position - (knockbackdirection * knockbackpower));

        Debug.Log("�m�b�N�o�b�N" + (m_tThisTrans.position - knockbackdirection));
        
    }

    //------------------------------------
    //���̉e���ݒ�֐�
    //�����F���̌���
    //�����F���̋���
    //------------------------------------
    public void WindMove(CS_Wind.E_WINDDIRECTION distination,float windpower)
    {
        
        Vector3 Direction = Vector3.zero;

        m_aThisAnimator.SetBool("Jump", false);

        //�����ɂ���ĕ�����ݒ�
        switch (distination)
        {
            case CS_Wind.E_WINDDIRECTION.NONE:
                Direction = Vector3.zero;
                break;
            case CS_Wind.E_WINDDIRECTION.LEFT:
                Direction = Vector3.right;
                //���ړ��A�j���[�V�������Đ�
                m_aThisAnimator.SetBool("Left", true);
                break;
            case CS_Wind.E_WINDDIRECTION.RIGHT:
                Direction = Vector3.left;
                //�E�ړ��A�j���[�V�������Đ�
                m_aThisAnimator.SetBool("Right", true);
                break;
            case CS_Wind.E_WINDDIRECTION.UP:
                Direction = Vector3.up;

                //���V�A�j���[�V�������Đ�
                m_aThisAnimator.SetBool("Fry", true);
                break;
        }

        m_rThisRigidbody.AddForce(Direction * windpower, ForceMode2D.Force);

    }


    //------------------------------------
    //�S�[���֐�
    //------------------------------------
    private void OnGoal()
    {
        //�A�j���[�V�����Đ�
    }


    //-----------------------------------------------
    // ���V�A�j���[�V�����̏I��(Animation�ŌĂяo��)
    //-----------------------------------------------
    private void WindAnimEnd()
    {
        m_aThisAnimator.SetBool("Jump", true);
        m_aThisAnimator.SetBool("Fry", false);
    }

    //-----------------------------------------------
    // �E���ړ��A�j���[�V�����̏I��(Animation�ŌĂяo��)
    //-----------------------------------------------
    private void MoveAnimEnd()
    {
        m_aThisAnimator.SetBool("Jump", true);
        m_aThisAnimator.SetBool("Left", false);
        m_aThisAnimator.SetBool("Right",false);
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
