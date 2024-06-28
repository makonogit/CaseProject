//------------------------------------
//担当者：菅眞心
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//------------------------------------
//障害物クラス
//------------------------------------
public class CS_Obstacle : MonoBehaviour
{
    [SerializeField, Header("ノックバックの強さ")]
    private float m_fKnockBackForce = 1.0f;

    [SerializeField, Header("減速率")]
    private float m_fSpeedDownRate = 1.0f;

    [SerializeField, Header("攻撃力")]
    private float m_fAttackPower = 0.0f;

    private float m_fNowUpPower = 0.0f;

    [SerializeField, Header("自身のTarnsform")]
    private Transform m_tThisTrans;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //プレイヤーと衝突したら減速させる
        if (collision.transform.tag == "Player")
        {
            m_fNowUpPower = collision.transform.GetComponent<CS_Player>().UPPOWER;
            collision.transform.GetComponent<CS_Player>().UPPOWER = -m_fSpeedDownRate;
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーと衝突したら減速させる
        if (collision.transform.tag == "Player")
        {

            collision.transform.GetComponent<CS_Player>().UPPOWER = -collision.transform.GetComponent<CS_Player>().UPPOWER;
            collision.transform.GetComponent<CS_Player>().UPPOWER = m_fNowUpPower;
        }
    }

    //当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーと衝突したらノックバックさせる
        if(collision.transform.tag == "Player")
        {
            //方向を求めて方向と力を設定
            Vector3 Direction = m_tThisTrans.position - collision.transform.position;
            collision.transform.GetComponent<CS_Player>().KnockBack(Direction,m_fKnockBackForce);
        }
    }
}
