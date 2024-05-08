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
    [SerializeField, Header("現在のワールド番号")]
    private int m_nNowWorldNum = 1;

    [SerializeField, Header("現在のステージ番号")]
    private int m_nNowStageNum = 1;

    [Header("ステージデータ")]
    public List<World> m_Worlds;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
