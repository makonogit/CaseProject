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

    [SerializeField, Header("ゴール移動速度")]
    private float m_fGoalSpeed = 0.1f;

    [SerializeField, Header("ゴール用の星座背景")]
    private SpriteRenderer m_srSign;

    [SerializeField, Header("ゴール用の星座UI")]
    private SpriteRenderer m_srSignComplete;

    [SerializeField, Header("ゴール表示速度")]
    private float m_fGoalSignViewSpeed = 2.0f;

    private float m_fTimeMesure = 0.0f; //時間計測用

    private float m_fGoalSignAlpha = 0.0f;  //ゴール用背景の透明度

    private bool m_IsGoalView = false;      //ゴール時の表示フラグ(なんかきもいからなおしたい
    private bool m_IsLightChange = false;   //明転フラグ

    private bool m_IsGoal = false;

    private bool m_IsStarSpot = false;

    void Start()
    {
        
        if (!m_tGoleTrans) { Debug.LogWarning("ゴールTransformが設定されていません"); }
        if (!m_srSign) { Debug.LogWarning("星座Spiterenderが設定されていません"); }
        if (!m_srSignComplete) { Debug.LogWarning("星座完成UIのSpiterenderが設定されていません"); }

        m_srSign.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);
        m_srSignComplete.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        //ゴールしてなかったら更新しない
        if (!m_IsGoal) { return; }

        //if(m_tPlayerTrans.position == m_tGoleTrans.position) { return; }

        //if (m_tPlayerTrans.localScale != m_tPlayerTrans.localScale) { return; }

        //目的地に達したらゴール用オブジェクト表示開始
        if (ObjectData.m_tStarChildTrans.position == m_tGoleTrans.position) 
        {
            m_IsStarSpot = true;
        }
        else
        {
            //ゴール座標まで移動
            ObjectData.m_tStarChildTrans.position = Vector3.MoveTowards(ObjectData.m_tStarChildTrans.position, m_tGoleTrans.position, m_fGoalSpeed * Time.deltaTime);

            //ゴールの星と同じようにスケール縮小
           // ObjectData.m_tStarChildTrans.localScale = Vector3.MoveTowards(ObjectData.m_tStarChildTrans.localScale,/* m_tGoleTrans.localScale*/Vector3.zero, m_fGoalSpeed / 10 * Time.deltaTime);
        }

        if(m_IsStarSpot)
        {

            //星がはまった時の音を再生
            ObjectData.m_csSoundData.PlaySE("StarFit");
            //環境光を徐々に明るく
            if (!m_IsLightChange && ObjectData.m_lGlobalLight.intensity < 5.0f) { ObjectData.m_lGlobalLight.intensity += 2.0f * Time.deltaTime; }
            else { m_IsLightChange = true; }

            if (m_IsLightChange)
            {
                if (ObjectData.m_lGlobalLight.intensity > 1.0f) { ObjectData.m_lGlobalLight.intensity -= 2.0f * Time.deltaTime; }
                else
                {
                    //星の子を消してスプライトの差し替え
                    ObjectData.m_tStarChildTrans.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                    m_IsGoalView = true;
                }
            }

        }

        if (m_IsGoalView)
        {
            //完成SE再生
            ObjectData.m_csSoundData.PlaySE("SignComplete");

            //時間計測がはじまってなかったら
            if (m_fTimeMesure == 0.0f)
            {
                //ゴール用の背景を徐々に表示
                m_fGoalSignAlpha += m_fGoalSignViewSpeed * Time.deltaTime;

                m_srSign.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);
                m_srSignComplete.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);
            }

            //表示しきったら少し待機
            if (m_fGoalSignAlpha > 1.0f)
            {
                m_fTimeMesure += Time.deltaTime;
               
            }

            //待機終了したらシーン移動
            if (m_fTimeMesure > 2.5f)
            {
                SceneManager.LoadScene("SelectScene");
            }
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            //制限時間UI表示
           //SObjectData.m_csTimeLimit.transform.parent.gameObject.SetActive(false);

            //追記：中島2024.04.03
            //ゲームオーバーフラグをfalseに設定
            //CS_ResultController.GameOverFlag = false;
            //SceneManager.LoadScene("Result");

            //星が移動するSE再生
            ObjectData.m_csSoundData.StopBGM();
            ObjectData.m_csSoundData.PlaySE("StarMove");

            Debug.Log("ゴール");

            ObjectData.m_csCamCtrl.TARGET = ObjectData.m_tStarChildTrans.gameObject;

            //重力を無効
            Rigidbody2D playerd = ObjectData.m_tPlayerTrans.GetComponent<Rigidbody2D>();
            playerd.constraints = RigidbodyConstraints2D.FreezeAll;

            //ゴール判定
            m_IsGoal = true;

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
    }

}
