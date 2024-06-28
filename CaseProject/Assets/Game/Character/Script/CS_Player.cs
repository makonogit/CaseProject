//------------------------------------
//�S���ҁF�����S
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    [SerializeField, Header("���̎qTransForm")]
    private Transform m_tStarChildTrans;

    [SerializeField, Header("�V���E�X�̃A�j���[�^�[")]
    private Animator m_aThisAnimator;

    [SerializeField, Header("�ő�㏸���x")]
    private float m_fMaxUpSpeed = 10.0f;

    [SerializeField, Header("�㏸�{��")]
    private float m_fUpPower = 5.0f;

    [SerializeField, Header("�����{��")]
    private float m_fSpeedDown = 30.0f;

    [SerializeField, Header("���E�̈ړ����x")]
    private float m_fleftright = 3.0f;

    [SerializeField, Header("�ǂɓ����������̒��˕Ԃ�")]
    private float m_fWallReflect = 2.0f; 


    [SerializeField, Header("Effect�\���p�I�u�W�F�N�gTransform")]
    private Transform m_tEffectTrans;

    [SerializeField, Header("�G�t�F�N�g�\���pAnimator")] 
    private Animator m_aEffectAnim;

    [SerializeField, Header("�X�e�[�W��GlobalLight")]
    private Light2D m_lGlobalLight;


    private bool m_isUpTrigger = false;                //�㏸����
    private bool m_isSpeedDownTrigger = false;         //��������
    private Vector3 m_v3NowUpPower = Vector3.zero;     //���݂̏㏸��

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
        if (!m_tEffectTrans) Debug.LogWarning("Effect�I�u�W�F�N�g��Transform���ݒ肳��Ă��܂���");
        if (!m_aEffectAnim) Debug.LogWarning("Effect�p��Animator���ݒ肳��Ă��܂���");
        if (!m_tStarChildTrans) Debug.LogWarning("���̎qTransForm���ݒ肳��Ă��܂���");
        if (!m_lGlobalLight) Debug.LogWarning("GlobalLight���ݒ肳��Ă��܂���");

        m_fStartHeight = m_tThisTrans.position.y;   // �J�n���̍���

        //�Ǘ��N���X�Ƀf�[�^�ۑ�
        ObjectData.m_tPlayerTrans = m_tThisTrans;
        ObjectData.m_tStarChildTrans = m_tStarChildTrans;
        ObjectData.m_lGlobalLight = m_lGlobalLight;
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


        //���蔲���h�~(�ȈՎ���)
        if(m_tThisTrans.position.x > 8.8f)
        {
            Vector3 position = m_tThisTrans.position;
            position.x = 7.8f;
            m_tThisTrans.position = position;
        }
        if (m_tThisTrans.position.x < -8.8f)
        {
            Vector3 position = m_tThisTrans.position;
            position.x = -7.8f;
            m_tThisTrans.position = position;
        }


    }

    private void FixedUpdate()
    {
        //�肪�ӂ�ӂ퓮������
        m_tHandTrans.position = new Vector3(m_tHandTrans.position.x, m_tHandTrans.position.y + Wave, 0.0f);

        if (!m_isUpTrigger) { return; }

        ////���Ԃ܂ŏ㏸��������
        //m_fTimeMeasure += Time.deltaTime;

        //if(m_fTimeMeasure < m_fUpTime) 
        //{
        //    //�㏸�I��������p�����[�^��������
        //    m_fTimeMeasure = 0.0f;
        //    m_isUpTrigger = false;
        //    return; 
        //}

        //�ő呬�x�܂ŏ㏸�����猸������
        if(m_rThisRigidbody.velocity.magnitude > m_fMaxUpSpeed) 
        {
            m_rThisRigidbody.velocity = new Vector2(m_rThisRigidbody.velocity.x, m_fMaxUpSpeed);            
            m_isSpeedDownTrigger = true; }

        //��������
        if (m_isSpeedDownTrigger)
        {
            m_v3NowUpPower.y -= m_fSpeedDown * Time.deltaTime;

            Debug.Log(m_v3NowUpPower.y);

            //��������������I��
           if(m_v3NowUpPower.y < 0.0f)
           {
               m_isSpeedDownTrigger = false;
               m_isUpTrigger = false;
                return;
           }
            
        }

        m_rThisRigidbody.AddForce(m_v3NowUpPower, ForceMode2D.Force);

    }

    //�㏸���x�ύX
    public float UPPOWER
    {
        set
        {
            m_v3NowUpPower.y += value;
        }
        get
        {
            return m_v3NowUpPower.y;
        }
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

        //SE�Đ�
        ObjectData.m_csSoundData.PlaySE("KnockBack");

        //�m�b�N�o�b�N�̕����Ƌ������l�����ĖړI�n��ݒ�
        //m_v3DestinationPos = m_tThisTrans.position - (knockbackdirection * knockbackpower);

        //m_tThisTrans.position = m_v3DestinationPos;

        //Effect����(�Փ˂����ʒu�ōĐ�)
        m_tEffectTrans.position = new Vector3(m_tThisTrans.position.x + knockbackdirection.x, m_tThisTrans.position.y + knockbackdirection.y, 0.0f);
        m_aEffectAnim.SetTrigger("Damage"); //�_���[�WAnimation���Đ�

        Vector3 dir = m_tThisTrans.position - (knockbackdirection * knockbackpower);
        dir.y *= -5.0f;

        m_rThisRigidbody.AddForce(dir);
        //m_rThisRigidbody.velocity = m_tThisTrans.position - (knockbackdirection * knockbackpower);

        Debug.Log("�m�b�N�o�b�N" + (m_tThisTrans.position - knockbackdirection * knockbackpower));
        
    }

    //------------------------------------
    //���̉e���ݒ�֐�
    //�����F���̌���
    //�����F���̋���
    //------------------------------------
    public void WindMove(CS_Wind.E_WINDDIRECTION distination,float windpower)
    {
        
        Vector3 Direction = Vector3.zero;

        float power = windpower;

        m_aThisAnimator.SetBool("Jump", false);

        //�����ɂ���ĕ�����ݒ�
        switch (distination)
        {
            case CS_Wind.E_WINDDIRECTION.NONE:
                Direction = Vector3.zero;
                break;
            case CS_Wind.E_WINDDIRECTION.LEFT:
                Direction = Vector3.right;
                power *= m_fleftright;      //���ɔ{���������ĈЗ͂��グ��
                //���ړ��A�j���[�V�������Đ�
                m_aThisAnimator.SetBool("Left", true);
                break;
            case CS_Wind.E_WINDDIRECTION.RIGHT:
                Direction = Vector3.left;
                power *= m_fleftright;      //���ɔ{���������ĈЗ͂��グ��
                //�E�ړ��A�j���[�V�������Đ�
                m_aThisAnimator.SetBool("Right", true);
                break;
            case CS_Wind.E_WINDDIRECTION.UP:
                Direction = Vector3.up;

                power *= m_fUpPower;                //���ɔ{���������ĈЗ͂��グ��
                m_v3NowUpPower = Direction * power; //�͂�ۑ�
                m_isUpTrigger = true;               //�㏸���ɐݒ�

                //���V�A�j���[�V�������Đ�
                m_aThisAnimator.SetBool("Fry", true);
                return;
                break;
        }

        //m_rThisRigidbody.velocity = m_v3NowUpPower;

        //Debug.Log("�͂̂��킦�����" + Direction * windpower);
        m_rThisRigidbody.AddForce(Direction * power, ForceMode2D.Force);
        //m_rThisRigidbody.velocity = Direction * power;

    }


    //------------------------------------
    //�S�[���֐�
    //------------------------------------
    private void OnGoal()
    {
        //�A�j���[�V�����Đ�
        m_aThisAnimator.SetTrigger("Gole");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ǂɓ���������m�b�N�o�b�N
        if (collision.transform.tag == "Stage")
        {
            //Debug.Log("��");
            Vector3 dir = m_tThisTrans.position - collision.transform.position;
            dir.y = 0.0f;

            Vector3 force = m_tThisTrans.position - (dir * m_fWallReflect);

            m_rThisRigidbody.AddForce(force);
            //KnockBack(dir, m_fWallReflect);
        }
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
