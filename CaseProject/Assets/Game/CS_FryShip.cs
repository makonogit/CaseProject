//------------------------------------
//担当者：菅眞心
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_FryShip : MonoBehaviour
{
    private Transform m_tThisTransform;  //自身のTransform

    [SerializeField, Header("浮いている高さ")]
    private float m_fFloatHight = 0.1f;
    [SerializeField, Header("浮いている速度")]
    private float m_fFloatSpeed = 1.0f;

    private Vector3 m_v3StartPos;       //開始位置

    private bool m_isWindMove = false;  //風に影響を受けているか
    private float m_fWindPower;         //風の影響力
    private Quaternion m_qTargetAngle;  //影響によって傾く角度
    private Vector2 m_v2TargetVelocity; //影響によって進む方向



    // Start is called before the first frame update
    void Start()
    {
        m_tThisTransform = this.transform;
        m_v3StartPos = m_tThisTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //船のふわふわ浮いている表現
        float newY = m_v3StartPos.y + Mathf.Sin(Time.time * m_fFloatSpeed) * m_fFloatHight;
        m_tThisTransform.position = new Vector3(m_tThisTransform.position.x, newY, m_tThisTransform.position.z);

        //====風に影響される処理===
        if(m_isWindMove)
        {
            m_tThisTransform.rotation = Quaternion.Lerp(m_tThisTransform.rotation, m_qTargetAngle,Time.deltaTime * m_fWindPower);

            Vector2 currentpos = (Vector2)transform.position;

            Vector2 targetpos = currentpos + m_v2TargetVelocity * Time.deltaTime;

            transform.position = Vector2.Lerp(currentpos, targetpos, Time.deltaTime * (m_fWindPower * 100.0f));
        }

    }

    //風がすり抜ける瞬間
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
        {
            //風の影響力を取得する
            CS_Wind wind = collision.transform.GetComponent<CS_Wind>();
            m_fWindPower = wind.WindPower;

            //衝突した方向ベクトルを取得
            Vector2 direction = collision.transform.position - transform.position;
            direction.Normalize();

            //ベクトルから角度を計算
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90.0f; //オブジェクトが回転済みなので調整
            m_qTargetAngle = Quaternion.Euler(0, 0, angle);

            //進行方向の取得
            m_v2TargetVelocity = direction * -10.0f;

            //風の影響を受ける
            m_isWindMove = true;
        }
    }

    //すり抜け判定
    private void OnTriggerExit2D(Collider2D collision)
    {
        //風がすり抜けたら影響をなくす
        if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
        {
            m_isWindMove = false;
            Destroy(collision.gameObject);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.name == "Wind")
    //    {
         
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
        
    //}
}

