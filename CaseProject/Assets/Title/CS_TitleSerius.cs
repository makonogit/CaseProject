//-----------------------------------------------
//�S���ҁF��������
//�V���E�X�̏���
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleSerius : MonoBehaviour
{
    [SerializeField, Header("�^�C�g���Ǘ��X�N���v�g")]
    private CS_TitleHandler m_titleHandler;
    [SerializeField, Header("�ڕW�n�_")]
    private Transform m_target1;
    [SerializeField, Header("�X�N���[���I����ڕW�n�_2")]
    private Transform m_target2;
    
    [SerializeField, Header("�ړ����x")]
    private float m_speed;
    [SerializeField, Header("��]���x")]
    private float m_rotateSpeed = 50.0f;

    [SerializeField, Header("�ҋ@����")]
    private float m_waitTime = 2.0f;

    [Header("�^�C�g�����S")]
    [SerializeField] private GameObject m_titleLogo1;
    [SerializeField] private GameObject m_titleLogo2;

    [SerializeField, Header("�X�^�[�p�[�e�B�N��")]
    private GameObject m_starParticle;

    [SerializeField, Header("�g�嗦")]
    private float m_scaleFactor = 0.1f;
    [SerializeField, Header("�g��X�s�[�h")]
    private float m_scaleSpeed = 2;
    [SerializeField, Header("�ő�g��")]
    private float m_maxScale = 10;

    private float m_nowWaitTime = 0.0f;

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        //��]������
        this.transform.Rotate(Vector3.forward * m_rotateSpeed * Time.deltaTime);
        switch (m_titleHandler.TitleState)
        {
            case CS_TitleHandler.TITLE_STATE.BORN_SERIUS:
                //y���W���X�V
                Vector3 pos = this.transform.position;
                pos.y += m_speed * Time.deltaTime;
                this.transform.position = pos;
                //�^�[�Q�b�g�܂ł̃x�N�g��
                Vector3 targetToThis = m_target1.position - this.transform.position;
                //�x�N�g��y���}�C�i�X�H
                if(targetToThis.y < 0.0f)
                {
                    m_speed = 0.0f;//�X�s�[�h�𖳂���
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.WAIT1;//�ҋ@���1��
                }
                break;
            case CS_TitleHandler.TITLE_STATE.WAIT1:
                m_nowWaitTime += Time.deltaTime;//���݂̑ҋ@���Ԃ����Z

                //�ҋ@���Ԉ��ȏ�o�����H
                if(m_nowWaitTime >= m_waitTime)
                {
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.SCROLL;//�X�N���[����Ԃ�
                    m_nowWaitTime = 0.0f;
                    m_waitTime = 1.0f;
                }
                break;
            case CS_TitleHandler.TITLE_STATE.STOP:
                //���݂̑ҋ@���Ԃ��ҋ@���Ԃ�艺�H
                if (m_nowWaitTime <= m_waitTime)
                {
                    m_nowWaitTime += Time.deltaTime;//�f���^�^�C�������Z
                    if (m_nowWaitTime >= m_waitTime)
                    {
                        m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.MAGNIFICATION_SERIUS;
                    }
                    return;
                }
                break;
            case CS_TitleHandler.TITLE_STATE.MAGNIFICATION_SERIUS:
                if(transform.localScale.x < m_maxScale)
                {
                    transform.localScale += new Vector3(m_scaleFactor, m_scaleFactor, 0) * m_scaleSpeed * Time.deltaTime;
                    if(transform.localScale.x > m_maxScale)
                    {
                        Destroy(m_titleLogo1);
                        Destroy(m_starParticle);
                        m_titleLogo2.SetActive(true);
                        m_scaleSpeed *= 1.5f;
                        m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.REDUCTION_SERIUS;
                    }
                   
                }     
                break;
            case CS_TitleHandler.TITLE_STATE.REDUCTION_SERIUS:
                if (transform.localScale.x > 0.0f)
                {
                    transform.localScale -= new Vector3(m_scaleFactor, m_scaleFactor, 0) * m_scaleSpeed * Time.deltaTime;
                    if (transform.localScale.x < 0.0f)
                    {
                        Destroy(this.gameObject);
                        m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.WAIT2;
                    }
                }
                break;
        }
    }
}
