//-----------------------------------------------
//�S���ҁF���z��
//���C���[�`�F���W�N���X
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ChangeLayer : MonoBehaviour
{
    //���C���[���Ƃ̃f�[�^
    [System.Serializable]
    struct LayerData
    {
        //[SerializeField]
        public Vector3 Scale;
        //[SerializeField]
        public Vector3 Pos;
        //[SerializeField]
        public GameObject LayerObj;
    }

    [SerializeField, Header("���C���[���")]
    private LayerData[] m_Layer = new LayerData[3];

    private int m_nOldLayer = 1;    //�O�̃��C���[
    private int m_nNowLayer = 1;    //���݂̃��C���[

    [SerializeField, Header("��̃f�[�^�X�N���v�g")]
    private CS_HandPoseData m_handposedata;

    private bool m_isPush = false;      //���������̔���

    private float m_fTimer = 0.0f;      //�N�[���^�C��

    void Update()
    {
        int pushdata = m_handposedata.PUSHDATA;

        if ((pushdata == 1 || pushdata == 0) && !m_isPush)
        {
            m_isPush = true;

            //�X�V�O�̃��C���[��ۑ�
            m_nOldLayer = m_nNowLayer;

            //���݂̃��C���[���X�V
            m_nNowLayer = pushdata == 1 ? m_nNowLayer + 1 : m_nNowLayer - 1;

            //���C���[�̈ړ�����
            if (m_nNowLayer > 2) { m_nNowLayer = 2; }
            if (m_nNowLayer < 0) { m_nNowLayer = 0; }
        }


        if (m_isPush)
        {
            m_fTimer += Time.deltaTime;

            //���C���[���X�V����Ă�����
            if (m_nOldLayer < m_nNowLayer)    //����
            {
                if (m_nNowLayer == 2)
                {
                    //���̃��C���[���A�N�e�B�u�ɂ���
                    m_Layer[m_nNowLayer].LayerObj.SetActive(false);

                    //���C���[���P���ɂ���
                    m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer - 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.position, m_Layer[m_nOldLayer].Pos, Time.deltaTime);

                    m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);

                }

                if (m_nNowLayer == 1)
                {
                    //��O�̃��C���[���A�N�e�B�u�ɂ���
                    m_Layer[m_nNowLayer - 1].LayerObj.SetActive(true);

                    //���C���[���P���ɂ���
                    m_Layer[m_nNowLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.localScale, m_Layer[m_nOldLayer + 1].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.position, m_Layer[m_nOldLayer + 1].Pos, Time.deltaTime);

                    m_Layer[m_nNowLayer + 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer + 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer + 2].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer + 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer + 1].LayerObj.transform.position, m_Layer[m_nOldLayer + 2].Pos, Time.deltaTime);
                }

            }

            if (m_nOldLayer > m_nNowLayer)    //����
            {
                if (m_nNowLayer == 0)
                {
                    //��O�̃��C���[���A�N�e�B�u�ɂ���
                    m_Layer[m_nNowLayer].LayerObj.SetActive(false);

                    //���C���[���P�O�ɂ���
                    m_Layer[m_nOldLayer + 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer + 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer + 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer + 1].LayerObj.transform.position, m_Layer[m_nOldLayer].Pos, Time.deltaTime);

                    m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);
                }

                if (m_nNowLayer == 1)
                {
                    //���̃��C���[���A�N�e�B�u�ɂ���
                    m_Layer[m_nNowLayer + 1].LayerObj.SetActive(true);

                    //���C���[��1�O�ɂ���
                    m_Layer[m_nNowLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.localScale, m_Layer[m_nOldLayer - 1].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.position, m_Layer[m_nOldLayer - 1].Pos, Time.deltaTime);

                    m_Layer[m_nNowLayer - 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer - 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer - 2].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer - 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer - 1].LayerObj.transform.position, m_Layer[m_nOldLayer - 2].Pos, Time.deltaTime);
                }

            }

            //5�b��������
            if (m_fTimer > 5f)
            {
                m_fTimer = 0.0f;
                m_isPush = false;
            }
        }
    }
}
