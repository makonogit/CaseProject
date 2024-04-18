//-----------------------------------------------
//担当者：菅眞心
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_StageData : MonoBehaviour
{
    [SerializeField,Header("レイヤーオブジェクト")]
    private GameObject[] GameLayer = new GameObject[3];

    List<GameObject> m_EventObj;        //イベントオブジェクトリスト

    private int m_nStageEventNum = 0;   //ステージイベント数

    // Start is called before the first frame update
    void Start()
    {
        //全てのイベントを取得
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0;j<GameLayer[i].transform.childCount;j++)
            {   
                GameObject obj = GameLayer[i].transform.GetChild(j).gameObject;
                if (obj.tag == "Event") { m_EventObj.Add(obj); }
            }
        }

        //ステージイベント数保存
        m_nStageEventNum = m_EventObj.Count;
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    //イベント数取得関数
    //戻り値:イベント数
    public int GetAllEventNum()
    {
        return m_nStageEventNum;
    }

    //残りイベント数取得関数
    //戻り値:イベント数
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
