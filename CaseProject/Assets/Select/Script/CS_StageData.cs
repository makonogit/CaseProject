//-----------------------------------------------
//担当者：菅眞心
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_StageData : MonoBehaviour
{
    [Header("ステージデータ")]
    public List<World> m_Worlds;

    private int m_nMaxWorld = 0;    //ワールド最大数
    private int m_nMaxStage = 0;    //ステージ最大数

    private void Start()
    {
        //ワールド最大数の保存
        m_nMaxWorld = m_Worlds.Count;
        m_nMaxStage = m_Worlds[0].Stagedata.Count;
    }

    //ステージ情報Getter,Setter
    public int WORLDMAX
    {
        get
        {
            return m_nMaxWorld;
        }
    }

    public int STAGEMAX
    {
        get
        {
            return m_nMaxStage;
        }
        set
        {
            m_nMaxStage = value;
        }
    }
}

//-----------------------------------------------
//ステージデータクラス
//-----------------------------------------------
[System.Serializable]
public class StageData
{
    public Sprite m_sSelectStageSprite; //セレクト画面のステージスプライト
    public GameObject m_gStagePrefab;   //ステージのプレハブ
    public StageData(Sprite selectsprite,GameObject stagepbj)
    {
        m_gStagePrefab = stagepbj;
        m_sSelectStageSprite = selectsprite;
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


//-----------------------------------------------
//ステージ情報参照用クラス(シーン間の変数やり取り)
//-----------------------------------------------
public static class StageInfo
{
    [Header("ワールド番号")]
    public static int m_nWorldNum = 0;
    [Header("ステージ番号")]
    public static int m_nStageNum = 0;

    //-------------------------------------
    // ステージデータセット関数
    // 引数：ワールド番号
    // 引数：ステージ番号
    //-------------------------------------
    public static void SetStageData(int _worldnum,int _stagenum)
    {
        m_nWorldNum = _worldnum;
        m_nStageNum = _stagenum;
    }


    //---------------------------------------
    // ワールド番号Getter
    //---------------------------------------
    public static int World
    {
        get
        {
            return m_nWorldNum;
        }
    }


    //---------------------------------------
    // ステージ番号Getter
    //---------------------------------------
    public static int Stage
    {
        get
        {
            return m_nStageNum;
        }
       
    }

}

//-------------------------------------------
// ステージオブジェクト情報管理
// 違う方法で管理したいきもち,まこのきもち
//-------------------------------------------
public static class ObjectData
{

    [Header("星の子Transform")]
    public static Transform m_tStarChildTrans;

    [Header("PlayerTransform")]
    public static Transform m_tPlayerTrans;

    [Header("カメラ制御スクリプト")]
    public static CS_CameraControl m_csCamCtrl;
}