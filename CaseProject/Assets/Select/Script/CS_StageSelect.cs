//-----------------------------------------------
//担当者：菅眞心
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------
//ステージ選択クラス
//-----------------------------------------------
public class CS_StageSelect : MonoBehaviour
{

    //------------------------------------------------
    // 1-1,1-2と分かりやすいように1始まりで管理
    // 配列自体は0始まり
    [SerializeField, Header("現在のワールド番号")]
    private int m_nNowWorldNum = 1;

    [SerializeField, Header("現在のステージ番号")]
    private int m_nNowStageNum = 1;

    [SerializeField, Header("ステージ情報")]
    private CS_StageData m_csStageData;

    [SerializeField, Header("シーンマネージャー")]
    private CS_SceneManager m_csSceneManager;

    //-------------------------------------
    //　ステージを進める
    //　引数：何ステージ分更新するか(-1だったら1戻す)
    //-------------------------------------
    public void StageUpdate(int _stage)
    {
        //ワールドが最大(5-5)で値が正なら更新しない
        if(m_nNowWorldNum == m_csStageData.WORLDMAX && m_nNowStageNum == m_csStageData.STAGEMAX && _stage > 0) { return; }

        //ステージが最小(1-1)で値が負なら更新しない
        if(m_nNowWorldNum == 1 && m_nNowStageNum == 1 && _stage < 0) { return; } 

        m_nNowStageNum += _stage;

        //登録されているステージ数より大きい値になったらステージ更新せずワールド更新
        if(m_nNowStageNum > m_csStageData.STAGEMAX && m_nNowWorldNum < m_csStageData.WORLDMAX)
        {
            m_nNowWorldNum++;
            m_nNowStageNum = 1;

            //ワールド最大数の更新
            m_csStageData.STAGEMAX = m_csStageData.m_Worlds[m_nNowWorldNum - 1].Stagedata.Count;
        }

        //ステージデータを登録
        StageInfo.SetStageData(m_nNowWorldNum - 1, m_nNowStageNum - 1);

        Debug.Log("World:" + (StageInfo.World + 1) + "Stage:" + (StageInfo.Stage + 1));
    }

    // Start is called before the first frame update
    void Start()
    {
        //m_csSceneManager.LoadScene(CS_SceneManager.SCENE.GAME);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
