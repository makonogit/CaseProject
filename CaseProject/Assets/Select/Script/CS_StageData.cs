//-----------------------------------------------
//�S���ҁF�����S
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------
//�X�e�[�W�f�[�^�N���X
//-----------------------------------------------
[System.Serializable]
public class StageData
{
    public GameObject m_gSelectStagePrefab; //�Z���N�g��ʂ̃X�e�[�W�v���n�u
    public GameObject m_gStagePrefab;       //�X�e�[�W�̃v���n�u
    public StageData(GameObject selectobj,GameObject stagepbj)
    {
        m_gStagePrefab = stagepbj;
        m_gSelectStagePrefab = selectobj;
    }
}

//-----------------------------------------------
//���[���h�f�[�^�N���X
//-----------------------------------------------
[System.Serializable]
public class World
{
    public List<StageData> Stagedata;

    public World(List<StageData> stagedata)
    {
        Stagedata = stagedata;
    }
}


