//-----------------------------------------------
//担当者：菅眞心
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------
//ステージデータクラス
//-----------------------------------------------
[System.Serializable]
public class StageData
{
    public GameObject m_gSelectStagePrefab; //セレクト画面のステージプレハブ
    public GameObject m_gStagePrefab;       //ステージのプレハブ
    public StageData(GameObject selectobj,GameObject stagepbj)
    {
        m_gStagePrefab = stagepbj;
        m_gSelectStagePrefab = selectobj;
    }
}

//-----------------------------------------------
//ワールドデータクラス
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


