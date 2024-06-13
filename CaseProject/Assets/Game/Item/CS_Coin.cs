//-----------------------------------------------
//担当者：菅眞心
//コインギミック
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Coin : MonoBehaviour
{
    [SerializeField, Header("ステージの予測線")]
    private GameObject m_gPredictionLine;

    //予測線のSpiterenderRenderリスト
    private List<SpriteRenderer> m_srPredictionLineList = new List<SpriteRenderer>();

    [SerializeField, Header("予測線の表示時間")]
    private float m_fPredictionViewTime = 3.0f;

    //予測線の表示フラグ
    private bool m_isPredictionView = false;

    [SerializeField,Header("予測線の透明度")]
    private float m_fPredictionalpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //子オブジェクトのSpriRendererを全て取得
        //m_srPredictionLineList =　m_gPredictionLine.GetComponentsInChildren<SpriteRenderer>(m_srPredictionLineList);
        for (int i = 0; i < m_gPredictionLine.transform.childCount; i++)
        {
            Transform childtrans = m_gPredictionLine.transform.GetChild(i);
            SpriteRenderer sr = childtrans.GetComponent<SpriteRenderer>();
            m_srPredictionLineList.Add(sr);
        }

        //予測線を非表示に
        foreach (var sr in m_srPredictionLineList)
        {
            sr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (!m_isPredictionView) { return; }
        
        //時間計測
        m_fPredictionViewTime -= Time.deltaTime;
        

        //時間経過したら予測線を全て非表示にして終了
        if(m_fPredictionViewTime < 0.0f)
        {
            //予測線を非表示に
            foreach (var sr in m_srPredictionLineList)
            {
                sr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            Destroy(this.gameObject);
        }

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //プレイヤーが衝突したらアイテムの機能を無効化
        if(collision.transform.tag == "Player")
        {
            //SE再生
            ObjectData.m_csSoundData.PlaySE("Coin");

            this.GetComponent<CapsuleCollider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            //予測線の表示
            foreach(var sr in m_srPredictionLineList)
            {
                sr.color = new Color(1.0f, 1.0f, 1.0f, m_fPredictionalpha);
            }

            m_isPredictionView = true;
        }
    }

}
