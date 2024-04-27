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
    [SerializeField, Header("移動量")]
    private float m_fMove = 1.2f;

    private Vector3 m_v3StartPos;       //開始位置

    private bool m_isWindMove = false;  //風に影響を受けているか
    private float m_fWindPower;         //風の影響力
    private Quaternion m_qTargetAngle;  //影響によって傾く角度
    private Vector2 m_v2TargetVelocity; //影響によって進む方向
    private Vector3 m_v3WindVec;        //風の方向
    private float m_fSpeed = 0;

    [SerializeField,Header("最大HP")]
    private const float m_MaxHP = 100.0f;
    private float m_HP;                   //HP




    public float HP
    {
        set
        {
            m_HP = value;
        }
        get
        {
            return m_HP;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CS_HandSigns.OnCreateWinds += MoveByWind;
        m_tThisTransform = this.transform;
        m_v3StartPos = m_tThisTransform.position;

        m_HP = m_MaxHP; //HPを設定

    }

    // Update is called once per frame
    void Update()
    {
        //船のふわふわ浮いている表現
        Vector3 move = new Vector3(m_fMove * Time.deltaTime, fluffy, 0);
        m_tThisTransform.position += move +m_v3WindVec*Time.deltaTime;

        ////====風に影響される処理===
        //if(!m_isWindMove)
        //{
        //    //m_tThisTransform.rotation = Quaternion.Lerp(m_tThisTransform.rotation, m_qTargetAngle,Time.deltaTime * m_fWindPower);

        //    m_fSpeed += 2.5f *Time.deltaTime;
            
        //}
        //Vector2 currentpos = (Vector2)transform.position;

        //Vector2 targetpos = currentpos + m_v2TargetVelocity * m_fSpeed * Time.deltaTime;

        //transform.position = Vector2.Lerp(currentpos, targetpos, Time.deltaTime * (m_fWindPower * 100.0f));

        //m_fSpeed = Mathf.Min(0, m_fSpeed);

        if (m_v3WindVec.magnitude > 0) 
        {
            float length = m_v3WindVec.magnitude - 1.0f * Time.deltaTime;
            if(length < 0)length = 0;
            m_v3WindVec = m_v3WindVec.normalized * length;
        }
    }
    // 船がふわふわする時の移動値を返す
    private float fluffy{
        get {
            return Mathf.Sin(Time.time * m_fFloatSpeed) * m_fFloatHight;
        }
    }


    ////風がすり抜ける瞬間
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
    //    {
    //        //風の影響力を取得する
    //        CS_Wind wind = collision.transform.GetComponent<CS_Wind>();
    //        m_fWindPower = wind.WindPower;

    //        //衝突した方向ベクトルを取得
    //        Vector2 direction = collision.transform.position - transform.position;
    //        direction.Normalize();

    //        //ベクトルから角度を計算
    //        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //        //angle -= 90.0f; //オブジェクトが回転済みなので調整
    //        // m_qTargetAngle = Quaternion.Euler(0, 0, angle);

    //        //進行方向の取得
    //        m_v2TargetVelocity = direction;
    //        m_fSpeed = (m_fWindPower * -1);
    //        //風の影響を受ける
    //        m_isWindMove = true;
    //    }

    //    if(collision.gameObject.tag == "Cloud")
    //    {
    //        Debug.Log("aaaaaaaaaaaaaaaaaaa");
    //    }

    //}

    ////すり抜け判定
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    //風がすり抜けたら影響をなくす
    //    if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
    //    {
    //        m_isWindMove = false;
    //        Destroy(collision.gameObject);
    //    }
    //}

    // 風によって動く船が関数
    // 引数：特に意味なし
    // 引数：風の方向
    // 戻り値：なし
    private void MoveByWind(Vector3 pos,Vector3 dir) 
    {
        Vector3 normal = dir.normalized;
        normal.z = 0;
        normal.Normalize();

        float x = normal.x * normal.x;
        float y = normal.y * normal.y;
        
        // 垂直方向か
        bool isVertical =  x < y;
        if (isVertical) normal.x = 0;
        else normal.y = 0;
        normal.Normalize();

        float power = dir.magnitude * 0.5f;
        m_v3WindVec = normal*power;

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

