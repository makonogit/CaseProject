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

    private float m_fStartHeight;           //開始時の高さ
    private bool m_IsStartFry = false;      //浮遊開始したか
    
    [SerializeField, Header("自分のTransForm")]
    private Transform m_tThisTrans;

    [SerializeField, Header("自分のRigidbody")]
    private Rigidbody2D m_rThisRigidbody;

    [SerializeField, Header("シリウスのアニメーター")]
    private Animator m_aThisAnimator;

    [SerializeField, Header("移動速度")]
    private float m_fMoveSpeed = 1.0f;




    //--------------------
    //　手の動く処理用
    //--------------------
    [SerializeField, Header("手のTransForm")]
    private Transform m_tHandTrans;

    [SerializeField, Header("手の浮遊幅")]
    private float m_fHandFryLength = 0.1f;

    [SerializeField, Header("手の浮遊スピード")]
    private float m_fHandFrySpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_tThisTrans) Debug.LogWarning("シリウスのTransFormが設定されていません");
        if (!m_rThisRigidbody) Debug.LogWarning("シリウスのRigidBodyが設定されていません");
        if (!m_tHandTrans) Debug.LogWarning("シリウスの手のTransFormが設定されていません");
        if (!m_aThisAnimator) Debug.LogWarning("シリウスのAnimatorが設定されていません");

        m_fStartHeight = m_tThisTrans.position.y;   // 開始時の高さ
    }

    // Update is called once per frame
    void Update()
    {
        //m_v3DestinationPos = m_tThisTrans.position;

        //離陸したら離陸アニメーションの再生
        if (m_fStartHeight < m_tThisTrans.position.y && !m_IsStartFry) 
        {
            m_IsStartFry = true;
            m_aThisAnimator.SetBool("TakeOff", true); 
        }

        //手がふわふわ動く処理
        m_tHandTrans.position = new Vector3(m_tHandTrans.position.x, m_tHandTrans.position.y + Wave, 0.0f);

        

        //オブジェクトの向きを求める
        Vector3 Direction = m_v3DestinationPos - m_tThisTrans.position;

        //目的地に到着するまで移動
        if (m_v3DestinationPos != m_tThisTrans.position && m_v3DestinationPos != Vector3.zero)
        {
           
            //transform.position = Vector3.MoveTowards(transform.position, m_v3DestinationPos, m_fMoveSpeed * Time.deltaTime);
            //transform.Translate(0.0f, 0.1f, 0.0f);
        }
        else
        {
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_tThisTrans.position.x, 0.0f,0.0f), m_fMoveSpeed * Time.deltaTime);
            //transform.Translate(0.0f, -0.1f, 0.0f);
            //m_rThisRigidbody.AddForce(0.0f,m_fMoveSpeed);
        }

    }

    //ふわふわ動くようにSin波で値を返す
    private float Wave
    {
        get
        {
            return Mathf.Sin(Time.time * m_fHandFrySpeed) * m_fHandFryLength;
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
    //風の影響設定関数
    //引数：目的地
    //------------------------------------
    public void WindMove(CS_Wind.E_WINDDIRECTION distination,float windpower)
    {

        Vector3 Direction = Vector3.zero;

        //向きによって方向を設定
        switch(distination)
        {
            case CS_Wind.E_WINDDIRECTION.NONE:
                Direction = Vector3.zero;
                break;
            case CS_Wind.E_WINDDIRECTION.LEFT:
                Direction = Vector3.right;
                break;
            case CS_Wind.E_WINDDIRECTION.RIGHT:
                Direction = Vector3.left;
                break;
            case CS_Wind.E_WINDDIRECTION.UP:
                Direction = Vector3.up;
                break;
        }

        //風の方向と強さを考慮して目的地を設定
        //m_v3DestinationPos = m_tThisTrans.position + (Direction * windpower);

        m_rThisRigidbody.AddForce(Direction * windpower, ForceMode2D.Force);

        //離陸アニメーションを終了させて浮遊アニメーションを再生
        m_aThisAnimator.SetBool("Jump", false);
        m_aThisAnimator.SetBool("Fry", true);

        Debug.Log(windpower);
        Debug.Log(m_v3DestinationPos);
    }


    //------------------------------------
    //ゴール関数
    //------------------------------------
    private void OnGoal()
    {
        //アニメーション再生
    }


    //-----------------------------------------------
    // 風アニメーションの終了(Animationで呼び出し)
    //-----------------------------------------------
    private void WindAnimEnd()
    {
        m_aThisAnimator.SetBool("Jump", true);
        m_aThisAnimator.SetBool("Fry", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゴールに到着したらゴールイベントの発火
        if(collision.transform.tag == "Goal")
        {
            OnGoal();
        }

        //風に当たったら風の影響を受ける
        //if(collision.transform.tag == "Wind")
        //{
        //    CS_Wind wind = transform.GetComponent<CS_Wind>();
        //    if (!wind) return;

        //    float power = wind.WindPower;
        //    Debug.Log(power);
        //    Vector3 Direction = m_tThisTrans.position - collision.transform.position;
        //    WindMove(Direction, power);
        //}

    }


    //private void OnTriggerStay2D(Collider2D collision)
    //{

    //    //風に当たったら風の影響を受ける
    //    if (collision.transform.tag == "Wind")
    //    {
    //        CS_Wind wind = transform.GetComponent<CS_Wind>();
    //        if (!wind) return;

    //        float power = wind.WindPower;
            
    //        Vector3 Direction = m_tThisTrans.position - collision.transform.position;
    //        WindMove(Direction, power);
    //    }

    //}

}
