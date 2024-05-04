//-----------------------------------------------
//�S���ҁF��������
//�w�i�̃V�t�g����
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BackGroundLoop : MonoBehaviour
{
    [SerializeField,Header("�w�i�摜3��")]
    private GameObject[] m_backGrounds = new GameObject[3]; //�w�i�̔z��
    [SerializeField, Header("�S�[��")]
    private GameObject m_goalBackGround;
    [SerializeField, Header("�S�[�����o�b�N�O���E���h�̉����ڐ悩")]
    private int m_GoalBackNum;

    [SerializeField, Header("�^�C�g���Ǘ��X�N���v�g")]
    private CS_TitleHandler m_titleHandler;

    private Camera mainCamera;
    private float m_backgroundHeight; //�w�i�̍���

    private void Awake()
    {
        mainCamera = Camera.main;//���C���J�������擾
        //�w�i�摜�̍������擾
        m_backgroundHeight = m_backGrounds[0].GetComponent<SpriteRenderer>().bounds.size.y;

        //�w�i�摜���c�ɓ��Ԋu�ɕ��ׂ�
        for (int i = 1; i < m_backGrounds.Length; i++)
        {
            Vector3 newPosition = m_backGrounds[0].transform.position + Vector3.up * i * m_backgroundHeight;
            m_backGrounds[i].transform.position = newPosition;
        }

        m_goalBackGround.transform.position = m_backGrounds[0].transform.position + Vector3.up * m_GoalBackNum * m_backgroundHeight;
    }
   

    void Update()
    {
        //�w�i�������Ȃ�I��
        if (m_backGrounds[0] == null) { return; }

        if (m_titleHandler.TitleState == CS_TitleHandler.TITLE_STATE.STOP)
        {
            DestroyBackObjcts();//�w�i������
            return;
        }

        //�w�i���X�N���[��������
        for (int i = 0; i < m_backGrounds.Length; i++)
        {
            GameObject background = m_backGrounds[i];
            //�J���������݂̔w�i�O�ɏo����A�w�i����ԏ�ɃV�t�g
            if (background.transform.position.y + m_backgroundHeight * 1.5f < mainCamera.transform.position.y)
            {
                ShiftBackGround(background, i);
            }
        }
        
    }

   void ShiftBackGround(GameObject back,int num)
   {
        //��ԏ�ɂ���w�i�摜�̗v�f�ԍ����擾
        int top = (num + (m_backGrounds.Length -1)) % m_backGrounds.Length;
        //�V�����ʒu��ݒ�
        Vector3 newPos = m_backGrounds[top].transform.position + Vector3.up * m_backgroundHeight;
        m_backGrounds[num].transform.position = newPos;
   }

    void DestroyBackObjcts()
    {
        for (int i = 0; i < m_backGrounds.Length; i++)
        {
            Destroy(m_backGrounds[i]);
        }
        m_backGrounds[0] = null;
    }
}
