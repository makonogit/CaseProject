//------------------------------------
//担当者：菅眞心
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------
//ワープギミッククラス
//------------------------------------
public class CS_Warp : MonoBehaviour
{
    [SerializeField, Header("ワープ先オブジェクト")]
    private GameObject m_WarpObj;

    private bool m_IsWarp = false;  //ワープフラグ

    //public bool WARPFLG
    //{
    //    set
    //    {
    //        m_IsWarp = value;   
    //    }
    //    get
    //    {
    //        return m_IsWarp;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤー以外に当たったら終了
        if(collision.transform.tag != "Player") { return; }

        CS_Warp cswarp = m_WarpObj.GetComponent<CS_Warp>();
        
        //座標をそのまま入れ替え
        if(!m_IsWarp) collision.transform.position = m_WarpObj.transform.position;

        cswarp.m_IsWarp = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤー以外に当たったら終了
        if (collision.transform.tag != "Player") { return; }

        //ワープから抜けたらワープ状態終了
        CS_Warp cswarp = m_WarpObj.GetComponent<CS_Warp>();
        if(!cswarp.m_IsWarp)m_IsWarp = false;
    }


}
