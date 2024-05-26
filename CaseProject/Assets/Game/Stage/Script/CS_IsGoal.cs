//-----------------------------------------------
//担当者：菅眞心
//浮島(ゴール)クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using UnityEngine.SceneManagement;  //一旦直接SceneManagerを使用

public class CS_IsGoal : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField, Header("ゴールオブジェクト")]
    private Transform m_tGoleTrans;

    [SerializeField, Header("プレイヤーのTransform")]
    private Transform m_tPlayerTrans;

    [SerializeField, Header("プレイヤーのRigidbody")]
    private Rigidbody2D m_rPlayerRigid;

    [SerializeField, Header("星の子TransForm")]
    private Transform m_tStarChild;

    [SerializeField, Header("カメラ制御スクリプト")]
    private CS_CameraControl m_csCamCtrl;

    [SerializeField, Header("ゴール移動速度")]
    private float m_fGoalSpeed = 0.1f;
        
    private bool m_IsGoal = false;

    void Start()
    {
        
        if (!m_tGoleTrans) { Debug.LogWarning("ゴールTransformが設定されていません"); }
        if (!m_tPlayerTrans) { Debug.LogWarning("プレイヤーTransaformが設定されていません"); }
        if (!m_rPlayerRigid) { Debug.LogWarning("プレイヤーのRigidBodyが設定されていません"); }
        if (!m_tStarChild) Debug.LogWarning("星の子Transformが設定されていません");
        if (!m_csCamCtrl) { Debug.LogWarning("カメラ制御スクリプトが設定されていません"); }

    }

    // Update is called once per frame
    void Update()
    {
        //ゴールしてなかったら更新しない
        if (!m_IsGoal) { return; }

        //if(m_tPlayerTrans.position == m_tGoleTrans.position) { return; }

        //if (m_tPlayerTrans.localScale != m_tPlayerTrans.localScale) { return; }

        //ゴール座標まで移動
        m_tStarChild.position = Vector3.MoveTowards(m_tStarChild.position, m_tGoleTrans.position, m_fGoalSpeed * Time.deltaTime);

        //ゴールの星と同じようにスケール縮小
        m_tStarChild.localScale = Vector3.MoveTowards(m_tStarChild.localScale,/* m_tGoleTrans.localScale*/Vector3.zero, m_fGoalSpeed / 10 * Time.deltaTime);

        //目的地に達したらシーン遷移
        if (m_tPlayerTrans.position == m_tGoleTrans.position) 
        {
            SceneManager.LoadScene("SelectScene");
            return; 
        }
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

            m_csCamCtrl.TARGET = m_tStarChild.gameObject;

            //重力を無効
            m_rPlayerRigid.constraints = RigidbodyConstraints2D.FreezeAll;

            //ゴール判定
            m_IsGoal = true;

        }
    }

}
