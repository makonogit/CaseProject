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
                        m_speed = 5.0f;
                    }
                    return;
                }
                //�ڕW�l�̃x�N�g��������Ĉړ�
                Vector3 targetVec = (m_target2.position - transform.position);
                targetVec.z = 0.0f;
                Debug.Log("�^�[�Q�b�g" + targetVec.normalized);
                Vector3 newPos = transform.position + targetVec.normalized * m_speed * Time.deltaTime;
                transform.position = newPos;

                //�ڕW�Ƃ̃x�N�g���̒�������薢���H
                if(targetVec.magnitude < 0.5f)
                {
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.WAIT2;//�ҋ@���2��
                }

                break;
        }
    }
}
