//-----------------------------------------------
//担当者：中島愛音
//背景のシフト処理
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BackGroundLoop : MonoBehaviour
{
    [SerializeField,Header("背景画像3枚")]
    private GameObject[] m_backGrounds = new GameObject[3]; //背景の配列
    [SerializeField, Header("ゴール")]
    private GameObject m_goalBackGround;
    [SerializeField, Header("ゴールがバックグラウンドの何枚目先か")]
    private int m_GoalBackNum;

    [SerializeField, Header("タイトル管理スクリプト")]
    private CS_TitleHandler m_titleHandler;

    private Camera mainCamera;
    private float m_backgroundHeight; //背景の高さ

    private void Awake()
    {
        mainCamera = Camera.main;//メインカメラを取得
        //背景画像の高さを取得
        m_backgroundHeight = m_backGrounds[0].GetComponent<SpriteRenderer>().bounds.size.y;

        //背景画像を縦に等間隔に並べる
        for (int i = 1; i < m_backGrounds.Length; i++)
        {
            Vector3 newPosition = m_backGrounds[0].transform.position + Vector3.up * i * m_backgroundHeight;
            m_backGrounds[i].transform.position = newPosition;
        }

        m_goalBackGround.transform.position = m_backGrounds[0].transform.position + Vector3.up * m_GoalBackNum * m_backgroundHeight;
    }
   

    void Update()
    {
        //背景が無いなら終了
        if (m_backGrounds[0] == null) { return; }

        if (m_titleHandler.TitleState == CS_TitleHandler.TITLE_STATE.STOP)
        {
            DestroyBackObjcts();//背景を消す
            return;
        }

        //背景をスクロールさせる
        for (int i = 0; i < m_backGrounds.Length; i++)
        {
            GameObject background = m_backGrounds[i];
            //カメラが現在の背景外に出たら、背景を一番上にシフト
            if (background.transform.position.y + m_backgroundHeight * 1.5f < mainCamera.transform.position.y)
            {
                ShiftBackGround(background, i);
            }
        }
        
    }

   void ShiftBackGround(GameObject back,int num)
   {
        //一番上にある背景画像の要素番号を取得
        int top = (num + (m_backGrounds.Length -1)) % m_backGrounds.Length;
        //新しい位置を設定
        Vector3 newPos = m_backGrounds[top].transform.position + Vector3.up * m_backgroundHeight;
        m_backGrounds[num].transform.position = newPos;
   }

    void DestroyBackObjcts()
    {
        for (int i = 0; i < m_backGrounds.Length; i++)
        {
            Destroy(m_backGrounds[i]);
        }
        m_backGrounds[0] = null;
    }
}
