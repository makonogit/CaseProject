//------------------------------------
//担当者：菅眞心
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------
//プレイヤークラス
//------------------------------------
public class CS_Player : MonoBehaviour
{
    private float m_fMovement = 0.0f;       //移動量
    private Vector3 m_v3DestinationPos;     //移動目的地

    [SerializeField, Header("自分のTransForm")]
    private Transform m_tThisTrans;

    [SerializeField, Header("移動速度")]
    private float m_fMoveSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //m_v3DestinationPos = m_tThisTrans.position;

        //オブジェクトの向きを求める
        Vector3 Direction = m_v3DestinationPos - m_tThisTrans.position;

        //目的地に到着するまで移動
        if (m_v3DestinationPos != m_tThisTrans.position && m_v3DestinationPos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_v3DestinationPos, m_fMoveSpeed * Time.deltaTime);
        }
        else
        {
            m_v3DestinationPos = Vector3.zero;
        }

    }

    //------------------------------------
    //ノックバック関数
    //引数：ノックバックの方向,ノックバックの強さ
    //------------------------------------
    public void KnockBack(Vector3 knockbackdirection,float knockbackpower)
    {
        //ノックバックの方向と強さを考慮して目的地を設定
        m_v3DestinationPos = m_tThisTrans.position - (knockbackdirection * knockbackpower);
    }

    //------------------------------------
    //目的地設定関数
    //引数：目的地
    //------------------------------------
    private void SetDistination(Vector3 distination)
    {
        m_v3DestinationPos = distination;
    }


    //------------------------------------
    //ゴール関数
    //------------------------------------
    private void OnGoal()
    {
        //アニメーション再生
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゴールに到着したらゴールイベントの発火
        if(collision.transform.tag == "Goal")
        {
            OnGoal();
        }
    }

}
