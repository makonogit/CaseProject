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

    [SerializeField, Header("攻撃力")]
    private float m_fAttackPower = 0.0f;

    [SerializeField, Header("自身のTarnsform")]
    private Transform m_tThisTrans;

    //[SerializeField,Header("s")]
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
