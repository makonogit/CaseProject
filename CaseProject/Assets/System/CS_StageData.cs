//-----------------------------------------------
//�S���ҁF�����S
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_StageData : MonoBehaviour
{
    [SerializeField,Header("���C���[�I�u�W�F�N�g")]
    private GameObject[] GameLayer = new GameObject[3];

    List<GameObject> m_EventObj;        //�C�x���g�I�u�W�F�N�g���X�g

    private int m_nStageEventNum = 0;   //�X�e�[�W�C�x���g��

    // Start is called before the first frame update
    void Start()
    {
        //�S�ẴC�x���g���擾
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0;j<GameLayer[i].transform.childCount;j++)
            {   
                GameObject obj = GameLayer[i].transform.GetChild(j).gameObject;
                if (obj.tag == "Event") { m_EventObj.Add(obj); }
            }
        }

        //�X�e�[�W�C�x���g���ۑ�
        m_nStageEventNum = m_EventObj.Count;
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    //�C�x���g���擾�֐�
    //�߂�l:�C�x���g��
    public int GetAllEventNum()
    {
        return m_nStageEventNum;
    }

    //�c��C�x���g���擾�֐�
    //�߂�l:�C�x���g��
    public int GetEventNum()
    {
        int StageEventNum = 0;

        for(int i = 0; i<m_EventObj.Count;i++)
        {
            if (m_EventObj[i]) { StageEventNum++; }
        }

        return StageEventNum;

    }
}
