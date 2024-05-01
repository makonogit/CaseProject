//-----------------------------------------------
//担当者：菅眞心
//浮島(ゴール)クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_IsGoal : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField, Header("ゴールオブジェクト")]
    private Transform m_tGoleTrans;

    [SerializeField, Header("プレイヤーのTransform")]
    private Transform m_tPlayerTrans;

    [SerializeField, Header("ゴール移動速度")]
    private float m_fGoalSpeed = 0.1f;
        
    private bool m_IsGoal = false;

    void Start()
    {
        
        if (!m_tGoleTrans) { Debug.LogWarning("ゴールTransformが設定されていません"); }
        if (!m_tPlayerTrans) { Debug.LogWarning("プレイヤーTransaformが設定されていません"); }

    }

    // Update is called once per frame
    void Update()
    {
        //ゴールしてなかったら更新しない
        if (!m_IsGoal) { return; }

        //if(m_tPlayerTrans.position == m_tGoleTrans.position) { return; }

        //if (m_tPlayerTrans.localScale != m_tPlayerTrans.localScale) { return; }

        //ゴール座標まで移動
        m_tPlayerTrans.position = Vector3.MoveTowards(m_tPlayerTrans.position, m_tGoleTrans.position, m_fGoalSpeed * Time.deltaTime);

        //ゴールの星と同じようにスケール縮小
        m_tPlayerTrans.localScale = Vector3.MoveTowards(m_tPlayerTrans.localScale, m_tGoleTrans.localScale, m_fGoalSpeed / 2 * Time.deltaTime);


    }

    //--------------------------------------------
    // 曲線上の移動関数(ベジェ曲線)
    // 引数1：速度
    // 引数2：始点
    // 引数3：制御点
    // 引数4：終点
    //--------------------------------------------
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //追記：中島2024.04.03
            //ゲームオーバーフラグをfalseに設定
            //CS_ResultController.GameOverFlag = false;
            //SceneManager.LoadScene("Result");

            Debug.Log("ゴール");

            //ゴール判定
            m_IsGoal = true;

        }
    }

}
