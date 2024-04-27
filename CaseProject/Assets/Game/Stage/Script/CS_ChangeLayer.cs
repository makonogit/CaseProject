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

        public LayerData(Vector3 scale,Vector3 pos,GameObject obj)
        {
            Scale = scale;
            Pos = pos;
            LayerObj = obj;
        }

    }

    [SerializeField, Header("���C���[���")]
    private List<LayerData> m_Layer = new List<LayerData>();

    private int m_nOldLayer = 2;    //�O�̃��C���[
    private int m_nNowLayer = 2;    //���݂̃��C���[

    private const int m_MoveMax = 3;   //�ő僌�C���[��

    [SerializeField, Header("��̃f�[�^�X�N���v�g")]
    private CS_HandSigns m_handsigns;

    private bool m_isPush = false;      //���������̔���

    private float m_fTimer = 0.0f;      //�N�[���^�C��

    void Update()
    {
        
        int pushdata = 1;

        if(!m_isPush) pushdata = m_handsigns.PushHand();

        if ((pushdata == 1 && m_nNowLayer < m_MoveMax) || 
            (pushdata == 0 && m_nNowLayer > 1))
        {
            m_isPush = true;

            //�X�V�O�̃��C���[��ۑ�
            m_nOldLayer = m_nNowLayer;
            Debug.Log(pushdata);

            //���݂̃��C���[���X�V
            if(pushdata == 1)
            {
                m_nNowLayer++;
                for (int i = m_Layer.Count - 1; i < 1; i++)
                {
                    Debug.Log("I" + i);
                    m_Layer[i] = new LayerData(m_Layer[i].Scale, m_Layer[i].Pos, m_Layer[i - 1].LayerObj);
                }
            }
            if(pushdata == 0) 
            {
                m_nNowLayer--;
                for (int i = 0; i < m_Layer.Count - 1; i++)
                {
                    m_Layer[i] = new LayerData(m_Layer[i].Scale, m_Layer[i].Pos, m_Layer[i + 1].LayerObj);
                }
            }

            //���C���[�̈ړ�����
            //if (m_nNowLayer > m_LayerMax) { m_nNowLayer = m_LayerMax; }
            //if (m_nNowLayer < 0) { m_nNowLayer = 0; }

        }


        if (LayerChange())
        {
            m_isPush = false;
        }
 
    }


    private bool LayerChange()
    {
        //���C���[���X�V����Ă��Ȃ�������I��
     //   if (m_nOldLayer == m_nNowLayer) { return; }

        //�T�C�Y�ƈʒu�̍X�V
        for(int i = 0; i<m_Layer.Count;i++)
        {
            if(m_Layer[i].LayerObj == null) { continue; }
            m_Layer[i].LayerObj.transform.localScale =
                Vector3.Lerp(m_Layer[i].LayerObj.transform.localScale, m_Layer[i].Scale, Time.deltaTime);
            m_Layer[i].LayerObj.transform.position =
                Vector3.Lerp(m_Layer[i].LayerObj.transform.position, m_Layer[i].Pos, Time.deltaTime);
        }

        if(m_Layer[2].LayerObj.transform.position == m_Layer[2].Pos)
        {
            return true;
        }

        return false;
      
    }

}
