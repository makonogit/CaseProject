//-----------------------------------------------
//�S���ҁF���z��
//���C���[�`�F���W�N���X
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ChangeLayer : MonoBehaviour
{
    [SerializeField,Header("�g��E�k���W��")]
    private float m_fScaleFactor = 1.3f; // �g��E�k���̃X�P�[���t�@�N�^�[

    //��ԊǗ��ϐ�
    private int[] m_nScale = new int[3]{ -1, 0, 1 };
    private int m_nScaleState = 1;

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
    private LayerData[] m_Layer =�@new LayerData[3];

    private int m_nOldLayer = 1;
    private int m_nNowLayer = 1;    //���݂̃��C���[

    [SerializeField, Header("��̃f�[�^�X�N���v�g")]
    private CS_HandPoseData m_handposedata;

    private bool m_isPush = false;
    
    //�n���h�g���b�L���O����̓��͂ɕύX���镔��
    public KeyCode scaleKey01 = KeyCode.Space; // �g��E�k�����s���L�[
    public KeyCode scaleKey02 = KeyCode.Space; // �g��E�k�����s���L�[
    //---

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
            if(m_nNowLayer > 2) { m_nNowLayer = 2; }
            if(m_nNowLayer < 0) { m_nNowLayer = 0; }
        }


        if(m_isPush)
        {
            //���C���[���X�V����Ă�����
            if(m_nOldLayer < m_nNowLayer)    //����
            {
                if(m_nNowLayer == 2)
                {
                    m_Layer[m_nNowLayer].LayerObj.SetActive(false);
                    
                    m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale =
                    Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer].Scale, Time.deltaTime);

                    m_Layer[m_nOldLayer - 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.position, m_Layer[m_nOldLayer].Pos, Time.deltaTime);
                }

                if (m_nNowLayer == 1)
                {
                    m_Layer[m_nNowLayer - 1].LayerObj.SetActive(true);
                }

                m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                    Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);

                m_Layer[m_nOldLayer].LayerObj.transform.position =
                    Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);
            }

            if (m_nOldLayer > m_nNowLayer)    //����
            {
                if (m_nNowLayer == 0)
                {
                    m_Layer[m_nOldLayer + 1].LayerObj.SetActive(false);
                }

                if (m_nNowLayer == 1)
                {
                    m_Layer[m_nNowLayer - 1].LayerObj.SetActive(true);
                }

                m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                    Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);

                m_Layer[m_nOldLayer].LayerObj.transform.position =
                   Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);

            }

            if(m_Layer[m_nOldLayer].LayerObj.transform.localScale == m_Layer[m_nNowLayer].LayerObj.transform.localScale &&
               m_Layer[m_nOldLayer].LayerObj.transform.position == m_Layer[m_nNowLayer].LayerObj.transform.position)
            {
                m_isPush = false;
            }

        }

        {
            //// �S�ẴI�u�W�F�N�g���擾
            //GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

            //// �w�肵���L�[�������ꂽ��
            //if (Input.GetKeyDown(scaleKey01))
            //{
            //    if (m_nScale[2] > m_nScale[m_nScaleState])
            //    {
            //        m_nScaleState++;
            //        // �e�I�u�W�F�N�g�ɑ΂��ď������s��
            //        foreach (GameObject obj in objects)
            //        {
            //            // �I�u�W�F�N�g���v���C���[�łȂ��ꍇ�Ɋg��E�k�����s��
            //            if (!obj.CompareTag("Player"))
            //            {
            //                // �I�u�W�F�N�g�̃X�P�[����ύX����
            //                obj.transform.localScale *= m_fScaleFactor;
            //            }
            //        }

            //    }

            //}

            //if (Input.GetKeyDown(scaleKey02))
            //{
            //    if (m_nScale[0] < m_nScale[m_nScaleState])
            //    {
            //        m_nScaleState--;
            //        // �e�I�u�W�F�N�g�ɑ΂��ď������s��
            //        foreach (GameObject obj in objects)
            //        {
            //            // �I�u�W�F�N�g���v���C���[�łȂ��ꍇ�Ɋg��E�k�����s��
            //            if (!obj.CompareTag("Player"))
            //            {
            //                // �I�u�W�F�N�g�̃X�P�[����ύX����
            //                obj.transform.localScale /= m_fScaleFactor;
            //            }
            //        }
            //    }

            //}
        }
    }
}
